
namespace Web.HttpClients;

public interface ICreateMessage
{
    Task<long> GetDaterAsync(string text, int number);
}