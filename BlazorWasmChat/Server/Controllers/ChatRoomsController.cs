using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorWasmChat.Shared;

namespace BlazorWasmChat.Server.Controllers
{
    // Контроллер, предоставляющий список комнат
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ChatRoom> Get()
        {
            // Вернуть список комнат
            return Chat.Rooms.ToArray();
        }
    }
}
