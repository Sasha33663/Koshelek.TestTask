
using Web;
using Web.HttpClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();      // подключема сервисы SignalR
builder.Services.AddHttpClient<ICreateMessage,CreateMessage>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat");   // ChatHub будет обрабатывать запросы по пути /chat

app.Run();