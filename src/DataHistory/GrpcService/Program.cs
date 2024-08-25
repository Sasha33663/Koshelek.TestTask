using GrpcService;
using Microsoft.VisualBasic;
using Presentation;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var a = builder.Configuration.GetConnectionString("Receiver");
        var b = builder.Configuration["ConnectionString:Receiver"];
        builder.Services.AddGrpc();
        builder.Services.AddGrpcClient<HistoryService.HistoryServiceClient>(options=>options.Address=new Uri(b));
        builder.Services.AddGraphQLServer()
           .AddQueryType<DataHistoryController>();
        builder.Services.AddCors(x =>
        {
            x.AddDefaultPolicy(x =>
            {
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
            });
        });
        var app = builder.Build();
       
        app.MapGraphQL();
        app.Run();
    }
}