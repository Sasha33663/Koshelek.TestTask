using Application.Service;
using Domain;
using Microsoft.AspNetCore.Mvc;
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
    public ReceiverController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("create_message")]
    public async Task <Message> CreateMessageAsync([FromQuery]CreateMessageDto dto, CancellationToken token)
    {
      return  await _messageService.CreateMessageAsync(dto.Number,dto.Text, token);
    }
  
}
