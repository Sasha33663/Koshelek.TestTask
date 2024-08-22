using Application.Exeptions;
using Domain;
using Infrastructure.Repositories;
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
    public MessageService(IMessageRepository messageRepository, MessageValidation validate)
    {
        _messageRepository = messageRepository;
        _validate = validate;
    }

    public async Task CreateMessageAsync(int number, string text, CancellationToken token)
    {
        var numberValidate = await _messageRepository.CheckNumberAsync(number);
        await _validate.NumberValidateAsync(numberValidate);
        await _validate.TextValidateAsync(text);
        var message = new Message(number, text);
        await _messageRepository.CreateMessageAsync(message);
    } 
    
}
