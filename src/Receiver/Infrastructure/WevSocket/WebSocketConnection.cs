using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.WevSocket;
public class WebSocketConnection
{
    private readonly HubConnection _connection;

    public WebSocketConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("ws://localhost:5293/chat")
            .Build();

        _connection.On<string>("ReceiveMessage", (message) =>
        {
            Console.WriteLine($"Message received: {message}");
        });
    }

    public async Task StartAsync()
    {
        await _connection.StartAsync();
        Console.WriteLine("Connection started");
    }

    public async Task SendMessageAsync(string message)
    {
        await _connection.InvokeAsync("SendText", message);
    }
}
