using Microsoft.AspNetCore.SignalR;
using BlazorWasmChat.Shared;

namespace BlazorWasmChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(ChatMessage message)
        {
            // Отправить сообщение всем клиентам, чтобы они добавили
            // текст в свои хранилища сообщений
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
