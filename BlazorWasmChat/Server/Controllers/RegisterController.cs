using Microsoft.AspNetCore.Mvc;
using BlazorWasmChat.Shared;
using BlazorWasmChat.Server.Authorization;

namespace BlazorWasmChat.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		[HttpPost]
		public async Task<ActionResult<string>> AddUser(UserLogin request)
		{
			if (string.IsNullOrEmpty(request.UserName))
			{
				return "Требуется имя пользователя";
			}

			if (string.IsNullOrEmpty(request.Password))
			{
				return "Требуется пароль";
			}

			if (Chat.Users.Exists(x => x.UserName == request.UserName))
			{
				return "Пользователь с таким именем уже зарегистрирован";
			}

			string res;

			try
			{
				Chat.Users.Add(request);
				res = "OK";
			}
			catch (Exception ex)
			{
				res = "Ошибка регистрации: " + ex.Message;
			}

			return res;
		}
	}
}
