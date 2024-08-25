using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Json;

namespace Web.HttpClients;

public class CreateMessage : ICreateMessage
{
    private readonly HttpClient _httpClient;

    public CreateMessage(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<long> GetDaterAsync(string text, int number)
    {
        var requestMessage = $"https://localhost:7091/api/receiver/create_message";
        var query = new Dictionary<string, string>
        {
            ["Text"] = text,
            ["Number"] = number.ToString()
        };
        var uri = requestMessage + new QueryBuilder(query);
        var content = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(uri)
        };
        var responseMessage = await _httpClient.SendAsync(content);
        responseMessage.EnsureSuccessStatusCode();
        var message = await responseMessage.Content.ReadFromJsonAsync<Message>();
        return message.Date;
    }
}
