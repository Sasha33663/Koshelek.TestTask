using Application.Exeptions;
using Microsoft.Extensions.Logging;

namespace Application.Service;

public class MessageValidation
{
    private readonly ILogger<MessageValidation> _logger;

    public MessageValidation(ILogger<MessageValidation> logger)
    {
        _logger = logger;
    }

    public async Task NumberValidateAsync(int count)
    {
        _logger.LogDebug("Валидация номера: {Count} записей найдено.", count);

        if (count > 0)
        {
            _logger.LogWarning("Валидация номера не пройдена: номер уже существует.");
            throw new NumberException();
        }
    }

    public async Task TextValidateAsync(string text)
    {
        _logger.LogDebug("Валидация текста: длина {Length}.", text.Length);

        if (text.Length >= 128)
        {
            _logger.LogWarning("Валидация текста не пройдена: текст слишком длинный.");
            throw new TextException();
        }
    }
}