using GrpcService;
using Infrastructure.Repositories;
using Infrastructure.WevSocket;
using Microsoft.Extensions.Logging;

namespace Application.Service;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly MessageValidation _validate;
    private readonly WebSocketConnection _myClient;
    private readonly ILogger<MessageService> _logger;

    public MessageService(IMessageRepository messageRepository, MessageValidation validate, WebSocketConnection myClient, ILogger<MessageService> logger)
    {
        _messageRepository = messageRepository;
        _validate = validate;
        _myClient = myClient;
        _logger = logger;
    }

    public async Task<Domain.Message> CreateMessageAsync(int number, string text, CancellationToken token)
    {
        _logger.LogInformation("Начало создания сообщения с номером {Number} и текстом {Text}.", number, text);

        try
        {
            _logger.LogDebug("Проверка наличия номера в базе.");
            var numberValidate = await _messageRepository.CheckNumberAsync(number);

            _logger.LogDebug("Валидация номера.");
            await _validate.NumberValidateAsync(numberValidate);

            _logger.LogDebug("Валидация текста.");
            await _validate.TextValidateAsync(text);

            var message = new Domain.Message(number, text, DateTimeOffset.Now.ToUnixTimeSeconds());

            _logger.LogDebug("Создание сообщения в базе данных.");
            await _messageRepository.CreateMessageAsync(message);

            _logger.LogDebug("Отправка сообщения через WebSocket.");
            await _myClient.SendMessageAsync(message);

            _logger.LogInformation("Сообщение успешно создано с номером {Number}.", number);

            return message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании сообщения с номером {Number}.", number);
            throw;
        }
    }

    public async Task<GrpcService.GetHistoryResponse> GetHistoryAsync()
    {
        try
        {
            _logger.LogInformation("Получение истории сообщений за последние 10 минут.");

            var time = DateTimeOffset.Now.ToUnixTimeSeconds();
            var lastTenMin = time - 60 * 10;

            var response = await _messageRepository.GetMessagesAsync(time, lastTenMin);
            var messages = response.Select(x => new GrpcService.Message
            {
                Date = x.Date,
                Number = x.Number,
                Text = x.Text
            }).ToList();

            var historyResponse = new GetHistoryResponse();
            historyResponse.Messages.AddRange(messages);

            _logger.LogInformation("История сообщений успешно получена.");

            return historyResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении истории сообщений.");
            throw;
        }
    }
}