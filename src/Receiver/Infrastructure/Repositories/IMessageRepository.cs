using Domain;

namespace Infrastructure.Repositories;

public interface IMessageRepository
{
    Task<int> CheckNumberAsync(int number);

    Task CreateMessageAsync(Message message);

    Task<List<Message>> GetMessagesAsync(long time, long lastTenMin);
}