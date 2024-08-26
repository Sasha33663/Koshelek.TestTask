using GrpcService;
using Microsoft.VisualBasic;
using Presentation;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors(x =>
        {
            x.AddDefaultPolicy(x =>
            {
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
            });
        });

        var a = builder.Configuration.GetConnectionString("Receiver");
        var b = builder.Configuration["ConnectionString:Receiver"];
        builder.Services.AddGrpc();
        builder.Services.AddGrpcClient<HistoryService.HistoryServiceClient>(options=>options.Address=new Uri(b));
        builder.Services.AddGraphQLServer()
           .AddQueryType<DataHistoryController>();
        
        var app = builder.Build();
        app.UseCors(builder =>
          builder
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
        app.MapGraphQL();
        app.Run();
    }
}