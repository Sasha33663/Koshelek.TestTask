using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Web.HttpClients;

namespace Web;

public class ChatHub : Hub
{
    private readonly ICreateMessage _message;

    public ChatHub(ICreateMessage message)
    {
        _message = message;
    }

    public async Task SendText(Message message)
    {
        var date = await _message.GetDaterAsync(message.Text, message.Number);
        //var date = 64598372456;
        await Clients.All.SendAsync("Receive", message.Text, message.Number, date);
    }
   
}

public class Message
{
    public int Number { get; set; }
    public string Text { get; set; }
    public long Date { get; set; }
}
