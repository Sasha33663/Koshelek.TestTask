using GrpcService;
using Microsoft.AspNetCore.Components;
using Message = Domain.Message;

namespace Presentation;
public class DataHistoryController
{
    private readonly HistoryService.HistoryServiceClient _historyServiceClient;

    public DataHistoryController(HistoryService.HistoryServiceClient historyServiceClient)
    {
        _historyServiceClient = historyServiceClient;
    }
    public async Task<List<Message>> GetHistoryAsync()
    {
        try
        {
            var response =  _historyServiceClient.GetHistory(new GrpcService.Void());
            var messages = response.Messages.Select(x => new Message(x.Number, x.Text, x.Date)).ToList();

            return messages;

        }
        catch(Exception ex)
        {
            throw;
        }

    }
    
}

