
using Web;
using Web.HttpClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
//builder.Services.AddHttpClient<ICreateMessage, CreateMessage>();
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