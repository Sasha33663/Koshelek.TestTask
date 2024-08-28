using Application.Service;
using GrpcService;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.WevSocket;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Grpc;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Reflection;
internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
           .MinimumLevel.Information()
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("receiver.log")  
           .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080, listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
            });

            options.ListenAnyIP(8081, listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
            });
        });

        builder.Services.AddSerilog();
        builder.Services.AddSignalR();
        builder.Services.AddControllers();
        builder.Services.AddCors(x =>
        {
            x.AddDefaultPolicy(x =>
            {
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
            });
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<DbContext>();
        builder.Services.AddTransient<IMessageService, MessageService>();
        builder.Services.AddTransient<IMessageRepository, MessageRepository>();
        builder.Services.AddTransient< MessageValidation>();
        builder.Services.AddGrpc();
        builder.Services.AddTransient<WebSocketConnection>(x =>
        {
            var logger = x.GetRequiredService<ILogger<WebSocketConnection>>();
            var address = builder.Configuration.GetValue<string>("WebSocketConnectionString");
            return new WebSocketConnection(logger, address);
        });
        var app = builder.Build();
       
         app.UseSwagger();
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Receiver");
            c.RoutePrefix = string.Empty;
         });

        app.UseCors(builder =>
         builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGrpcService<DataHistoryServiceImpl>();
        app.MapControllers();
        var dbContext = app.Services.GetRequiredService<DbContext>();
        Seeder.Start(dbContext);

        app.Run();
    }
}