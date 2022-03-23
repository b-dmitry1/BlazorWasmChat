using Microsoft.AspNetCore.Mvc;
using BlazorWasmChat.Shared;
using BlazorWasmChat.Server.Authorization;

namespace BlazorWasmChat.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class AuthController : ControllerBase
	{
		[HttpPost]
		public async Task<ActionResult<string>> Login(UserLogin request)
		{
			var token = Auth.AuthorizeByNameAndPassword(request.UserName, request.Password);

			return token;
		}
	}
}
