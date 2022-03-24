using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.DirectoryServices;

namespace BlazorWasmChat.Server.Authorization
{
	public class Auth
	{
		private static readonly string _jwtPrivateKey = "2EF2D6BCB91D44E5877FF39200409E97";
		private static readonly string _jwtIssuer = "ChatServer";
		private static readonly int _jwtDays = 366; // Будем выдавать токен на 1 год

		private static SymmetricSecurityKey CreateSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtPrivateKey));
		}

		public static TokenValidationParameters MakeTokenValidationParameters()
		{
			// Создать параметры проверки токенов
			var parameters = new TokenValidationParameters
			{
				// Разрешить проверку ключа безопасности
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = CreateSecurityKey(),

				// Включить проверку издателя токена (это наш сервер)
				ValidateIssuer = true,
				ValidIssuer = _jwtIssuer,

				// Не нужно проверять получателя токена
				ValidateAudience = false,

				// Включить проверку времени дайствия
				ValidateLifetime = true
			};
			return parameters;
		}

		public static string AuthorizeByNameAndPassword(string username, string password)
		{
			// Здесь необходимо проверить имя пользователя и пароль
			// После чего можно сформировать и вернуть токен

			bool auth;

			auth =
				AuthActiveDirectory.IsUserInActiveDirectory(username, password) ||
				AuthLocal.TryAuthLocal(username, password);

			if (!auth)
			{ 
				return "";
			}

			var claims = new List<Claim>();

			// В этом месте добавляем наши произвольные значения
			claims.Add(new Claim(ClaimTypes.Name, username));

			var signingCredentials = new SigningCredentials(CreateSecurityKey(), SecurityAlgorithms.HmacSha256);

			var result = new JwtSecurityToken(
				issuer: _jwtIssuer,
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromDays(_jwtDays)),
				signingCredentials: signingCredentials);

			return new JwtSecurityTokenHandler().WriteToken(result);
		}
	}
}
