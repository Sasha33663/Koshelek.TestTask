using Application.Service;
using GrpcService;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.WevSocket;
using Presentation.Grpc;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSignalR();
        builder.Services.AddTransient<WebSocketConnection>();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<DbContext>();
        builder.Services.AddTransient<IMessageService, MessageService>();
        builder.Services.AddTransient<IMessageRepository, MessageRepository>();
        builder.Services.AddTransient< MessageValidation>();
        builder.Services.AddGrpc();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGrpcService<DataHistoryServiceImpl>();
        app.MapControllers();
        var dbContext = app.Services.GetRequiredService<DbContext>();
        Seeder.Start(dbContext);

        app.Run();
    }
}