using Serilog;
using Serilog.Events;
using Web.WebSocket;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Log.Logger = new LoggerConfiguration()
                   .Enrich.FromLogContext()
                  .MinimumLevel.Information()
                  .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .WriteTo.File("streamer.log")
                  .CreateLogger();
        builder.Services.AddSignalR();
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
        app.UseCors(builder =>
                 builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapHub<ChatHub>("/chat");

        app.Run();
    }
}