using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using BlazorWasmChat.Shared;

namespace BlazorWasmChat.Server.Hubs
{
    // Концентратор сообщений SignalR
	public class ChatHub : Hub
    {
        [Authorize]
        public async Task SendMessage(ChatMessage message)
        {
            // Отправить сообщение всем клиентам, чтобы они добавили
            // текст в свои хранилища сообщений
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
