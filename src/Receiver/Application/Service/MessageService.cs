using Application.Exeptions;
using Domain;
using Infrastructure.Repositories;
using Infrastructure.WevSocket;

//using Infrastructure.WevSocket;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service;
public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly MessageValidation _validate;
    private readonly WebSocketConnection _myClient;
    public MessageService(IMessageRepository messageRepository, MessageValidation validate, WebSocketConnection myClient )
    {
        _messageRepository = messageRepository;
        _validate = validate;
        _myClient = myClient;
    }

    public async Task <Message> CreateMessageAsync(int number, string text, CancellationToken token)
    {
        
        var numberValidate = await _messageRepository.CheckNumberAsync(number);
        await _validate.NumberValidateAsync(numberValidate);
        await _validate.TextValidateAsync(text);

        var message = new Message(number, text, DateTimeOffset.Now.ToUnixTimeSeconds());
        await _myClient.SendMessageAsync(message);
        await _messageRepository.CreateMessageAsync(message);
        return message;
    }

    public async Task<GrpcService.GetHistoryResponse> GetHistoryAsync()
    {
        var time = DateTimeOffset.Now.ToUnixTimeSeconds();
        var lastTenMin = time - 60 * 10;
        var response = await _messageRepository.GetMessagesAsync(time, lastTenMin);
        var messages = response.Select(x => new GrpcService.Message { Date = x.Date, Number = x.Number, Text = x.Text }).ToList();
        var historyResponse = new GrpcService.GetHistoryResponse();
        historyResponse.Messages.AddRange(messages);
        return historyResponse;
    }
}
 