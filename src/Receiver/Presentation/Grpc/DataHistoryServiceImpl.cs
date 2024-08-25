using Application.Service;
using Grpc.Core;
using GrpcService;

namespace Presentation.Grpc;

public class DataHistoryServiceImpl :HistoryService.HistoryServiceBase
{
    private readonly IMessageService _messageService;
    public DataHistoryServiceImpl(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public override async Task<GetHistoryResponse> GetHistory(GrpcService.Void request, ServerCallContext context)
    {
       return await _messageService.GetHistoryAsync();
    }
}
