using Application.Service;
using Grpc.Core;
using GrpcService;
using Microsoft.Extensions.Logging;

namespace Presentation.Grpc;

public class DataHistoryServiceImpl :HistoryService.HistoryServiceBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<DataHistoryServiceImpl> _logger;
    public DataHistoryServiceImpl(IMessageService messageService, ILogger<DataHistoryServiceImpl> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    public override async Task<GetHistoryResponse> GetHistory(GrpcService.Void request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Запрос истории сообщений получен через gRPC-сервис.");

            var historyResponse = await _messageService.GetHistoryAsync();

            _logger.LogInformation("История сообщений успешно отправлена клиенту.");

            return historyResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке запроса истории сообщений через gRPC-сервис.");
            throw;
        }
    }
}
