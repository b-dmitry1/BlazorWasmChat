using Microsoft.AspNetCore.Mvc;
using BlazorWasmChat.Shared;

namespace BlazorWasmChat.Server.Controllers
{
	// Регистрация нового пользователя
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		[HttpPost]
		public async Task<ActionResult<string>> AddUser(UserLogin request)
		{
			// Проверить полученные данные
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

			// Добавить пользователя в базу данных
			string res;
			try
			{
				if (Chat.AddUser(request))
				{
					res = "OK";
				}
				else
				{
					res = "Не удалось добавить пользователя";
				}
			}
			catch (Exception ex)
			{
				res = "Ошибка регистрации: " + ex.Message;
			}

			return res;
		}
	}
}
