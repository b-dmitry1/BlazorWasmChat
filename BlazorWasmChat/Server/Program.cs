using BlazorWasmChat.Server.Authorization;
using BlazorWasmChat.Server.Hubs;
using BlazorWasmChat.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Добавить основные сервисы
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Добавить Jwt
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = Auth.MakeTokenValidationParameters();
		options.Events = new JwtBearerEvents
        {
            // Для корректной работы авторизации в SignalR нам необходимо
            // вручную извлечь из строки запроса токен авторизации
            // и преобразовать его к исходному виду
            OnMessageReceived = context =>
            {
                var path = context.HttpContext.Request.Path;

                // Нам нужны только запросы к концентраторам сообщений
                // Добавьте здесь другие пути при необходимости
                if (path.StartsWithSegments("/chathub"))
                {
                    // Это запрос SignalR
                    // Получить токен из параметров строки запроса
                    var accessToken = context.Request.Query["access_token"].ToString();

                    // Удалить лишние символы
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.Length > 1 && accessToken.StartsWith("\""))
                    {
                        accessToken = accessToken.Substring(1, accessToken.Length - 2);
                    }

                    // Передать токен
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthenticationCore();

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

// Использовать Jwt
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Зарегистрировать ChatHub для обмена сообщениями при помощи SignalR
app.MapHub<ChatHub>("/chathub");

// Добавить тестовый защищенный документ
app.Map("/private", [Authorize] () =>
{
    return "Защищенное содержимое";
});

app.Run();

