using Microsoft.AspNetCore.Mvc;
using BlazorWasmChat.Shared;
using BlazorWasmChat.Server.Authorization;

namespace BlazorWasmChat.Server.Controllers
{
	// Контроллер авторизации
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		[HttpPost]
		public async Task<ActionResult<string>> Login(UserLogin user)
		{
			// Попробовать авторизовать пользователя и вернуть токен клиенту
			var token = Auth.AuthorizeByNameAndPassword(user);

			return await Task.FromResult(token);
		}
	}
}
