﻿using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Web.HttpClients;

namespace Web;

public class ChatHub : Hub
{
    //private readonly ICreateMessage _message;

    //public ChatHub(ICreateMessage message)
    //{
    //    _message = message;
    //}
    
    //public async Task SendMessage(Message message)
    //{
    //     await _message.GetDateAsync(message.Text, message.Number);
    //}
    public async Task ViewMessage(Message message)
    {
       await Clients.All.SendAsync("Receive", message.Text, message.Number, message.Date);
    }

}

public class Message
{
    public int Number { get; set; }
    public string Text { get; set; }
    public long Date { get; set; }
}
