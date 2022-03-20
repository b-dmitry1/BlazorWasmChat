using BlazorWasmChat.Server.Hubs;
using BlazorWasmChat.Shared;

// Данные приложения
var Rooms = new List<ChatRoom>();

var builder = WebApplication.CreateBuilder(args);

// Добавить основные сервисы
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Добавить SignalR
builder.Services.AddSignalR();

var app = builder.Build();


// Обработка ошибок
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

// Статичное содержимое и исполняемая среда
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// Маршрутизация запросов
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Зарегистрировать ChatHub для обмена сообщениями при помощи SignalR
app.MapHub<ChatHub>("/chathub");

app.Run();
