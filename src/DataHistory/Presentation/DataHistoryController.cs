using GrpcService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Message = Domain.Message;

namespace Presentation;
public class DataHistoryController
{
    private readonly HistoryService.HistoryServiceClient _historyServiceClient;
    private readonly ILogger<DataHistoryController> _logger;
    public DataHistoryController(HistoryService.HistoryServiceClient historyServiceClient, ILogger<DataHistoryController> logger)
    {
        _historyServiceClient = historyServiceClient;
        _logger = logger;
    }
    public async Task<List<Message>> GetHistoryAsync()
    {
        try
        {
            _logger.LogInformation("Запрос истории сообщений через gRPC.");

            var response = await _historyServiceClient.GetHistoryAsync(new GrpcService.Void());
            var messages = response.Messages.Select(x => new Message(x.Number, x.Text, x.Date)).ToList();

            _logger.LogInformation("История сообщений успешно получена.");

            return messages;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при запросе истории сообщений через gRPC.");
            throw;
        }

    }
    
}

