using Domain;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.WevSocket;
public class WebSocketConnection
{
    private readonly HubConnection _connection;
    private readonly ILogger<WebSocketConnection> _logger;
    public WebSocketConnection(ILogger<WebSocketConnection> logger)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("ws://localhost:5293/chat")
            .Build();

        _connection.On<string>("ReceiveMessage", (message) =>
        {
            Console.WriteLine($"Message received: {message}");
        });
        _logger = logger;
    }



    public async Task SendMessageAsync(Message message)
    {

        try
        {
            await _connection.StartAsync();
            _logger.LogInformation("Соединение с сервером установлено.");
            await _connection.InvokeAsync("ViewMessage", message);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при установке соединения.");
            throw;
        }
    }
}
