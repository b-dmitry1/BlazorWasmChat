using BlazorWasmChat.Server.Hubs;
using BlazorWasmChat.Shared;

// ������ ����������
var Rooms = new List<ChatRoom>();

var builder = WebApplication.CreateBuilder(args);

// �������� �������� �������
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// ���������������� ChatHub ��� ������ ����������� ��� ������ SignalR
app.MapHub<ChatHub>("/chathub");

app.Run();
