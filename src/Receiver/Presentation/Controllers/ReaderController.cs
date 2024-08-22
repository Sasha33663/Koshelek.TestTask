using Application.Service;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers;
[Route("api/reader")]
public class ReaderController : Controller
{
    private readonly IMessageService _messageService;
    public ReaderController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("create_message")]
    public async Task CreateMessageAsync([FromForm]CreateMessageDto dto, CancellationToken token)
    {
        await _messageService.CreateMessageAsync(dto.Number,dto.Text, token);
    }
   
}
