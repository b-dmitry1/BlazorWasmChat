using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorWasmChat.Shared;

namespace BlazorWasmChat.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ChatRoom> Get()
        {
            return Chat.Rooms.ToArray();
        }
    }
}
