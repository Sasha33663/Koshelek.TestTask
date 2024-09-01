using GrpcService;

namespace Application.Service;

public interface IMessageService
{
    Task<Domain.Message> CreateMessageAsync(int number, string text, CancellationToken token);

    Task<GetHistoryResponse> GetHistoryAsync();
}