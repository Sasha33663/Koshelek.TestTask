using Application.Service;
using GrpcService;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.WevSocket;
using Presentation.Grpc;
using Serilog;
using Serilog.Events;
internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
           .MinimumLevel.Information()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("receiver.log")  
           .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSerilog();
        builder.Services.AddSignalR();
        builder.Services.AddTransient<WebSocketConnection>();
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
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
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