using Application.Service;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers;
[Route("api/receiver")]
public class ReceiverController : Controller
{
    private readonly IMessageService _messageService;
    private readonly ILogger<ReceiverController> _logger;
    public ReceiverController(IMessageService messageService, ILogger<ReceiverController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    [HttpPost("create_message")]
    public async Task <Message> CreateMessageAsync([FromForm]CreateMessageDto dto, CancellationToken token)
    {
        _logger.LogInformation("\nПолучен новый запрос на создание сообщения: Номер: {Number} Текст: {Text}",dto.Number,dto.Text);
      return  await _messageService.CreateMessageAsync(dto.Number,dto.Text, token);
    }
  
}
