
using Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddGraphQLServer()
            .AddQueryType<DataHistoryController>();

        var app = builder.Build();

        app.MapGraphQL();

        app.Run();
    }
}