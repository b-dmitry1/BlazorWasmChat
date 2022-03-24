using BlazorWasmChat.Server.Authorization;
using BlazorWasmChat.Server.Hubs;
using BlazorWasmChat.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// �������� �������� �������
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// �������� Jwt
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = Auth.MakeTokenValidationParameters();
		options.Events = new JwtBearerEvents
        {
            // ��� ���������� ������ ����������� � SignalR ��� ����������
            // ������� ������� �� ������ ������� ����� �����������
            // � ������������� ��� � ��������� ����
            OnMessageReceived = context =>
            {
                var path = context.HttpContext.Request.Path;

                // ��� ����� ������ ������� � �������������� ���������
                // �������� ����� ������ ���� ��� �������������
                if (path.StartsWithSegments("/chathub"))
                {
                    // ��� ������ SignalR
                    // �������� ����� �� ���������� ������ �������
                    var accessToken = context.Request.Query["access_token"].ToString();

                    // ������� ������ �������
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.Length > 1 && accessToken.StartsWith("\""))
                    {
                        accessToken = accessToken.Substring(1, accessToken.Length - 2);
                    }

                    // �������� �����
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthenticationCore();

// �������� SignalR
builder.Services.AddSignalR();

var app = builder.Build();


// ��������� ������
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

// ��������� ���������� � ����������� �����
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// ������������� ��������
app.UseRouting();

// ������������ Jwt
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// ���������������� ChatHub ��� ������ ����������� ��� ������ SignalR
app.MapHub<ChatHub>("/chathub");

// �������� �������� ���������� ��������
app.Map("/private", [Authorize] () =>
{
    return "���������� ����������";
});

app.Run();

