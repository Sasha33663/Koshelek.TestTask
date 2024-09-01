using Microsoft.AspNetCore.SignalR;

namespace Web.WebSocket;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
    }

    public async Task ViewMessage(Message message)
    {
        _logger.LogInformation("Получено сообщение : Текст: {MessageText} Номер: {MessageNumber} Дата: {MessageDate}",
                                 message.Text, message.Number, message.Date);

        try
        {
            await Clients.All.SendAsync("Receive", message.Text, message.Number, message.Date);
            _logger.LogInformation("Сообщение успешно отправлено всем клиентам.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при отправке сообщения клиентам.");
            throw;
        }
    }
}