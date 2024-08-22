
using Domain;

namespace Infrastructure.Repositories;

public interface IMessageRepository
{
    Task <int> CheckNumberAsync(int number);
    Task CreateMessageAsync(Message message);
}