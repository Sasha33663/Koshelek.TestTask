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
        //builder.Services.AddCors(options =>
        //{
        //    options.AddPolicy("AllowSpecificOrigin",
        //        builder => builder
        //            .AllowAnyOrigin() // –азрешаем запросы только с этого домена
        //            .AllowAnyHeader()
        //            .AllowAnyMethod());
        //});
        builder.Services.AddSignalR();
        builder.Services.AddTransient<WebSocketConnection>();
        builder.Services.AddControllers();

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