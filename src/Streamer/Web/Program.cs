
using Web;
using Web.HttpClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddHttpClient<ICreateMessage, CreateMessage>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapHub<ChatHub>("/chat");   

app.Run(); 