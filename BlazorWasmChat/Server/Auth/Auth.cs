using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.DirectoryServices;
using BlazorWasmChat.Shared;

namespace BlazorWasmChat.Server.Authorization
{
	// Общий интерфейс наших модулей-поставщиков авторизации
	public interface IChatAuthProvider
	{
		bool TryAuthorize(UserLogin user);
	}

	public class Auth
	{
		// Параметры токенов
		// Секретный ключ
		private static readonly string _jwtPrivateKey = "2EF2D6BCB91D44E5877FF39200409E97";
		// Название издателя
		private static readonly string _jwtIssuer = "ChatServer";
		// Срок действия токена
		private static readonly int _jwtDays = 366; // Будем выдавать токен на 1 год

		private static SymmetricSecurityKey CreateSecurityKey()
		{
			// Создадим симметричный ключ из строки секретного ключа
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

		public static string AuthorizeByNameAndPassword(UserLogin user)
		{
			// Здесь необходимо проверить имя пользователя и пароль
			// После чего можно сформировать и вернуть токен

			var auth = false;

			// Проверим всех наших провайдеров
			foreach (var provider in Chat.AuthProviders)
			{
				auth |= provider.TryAuthorize(user);
				if (auth)
				{
					break;
				}
			}

			if (!auth)
			{
				// Ни один из модулей не смог выполнить авторизацию
				return string.Empty;
			}

			// Реквизиты пользователя
			var claims = new List<Claim>();

			// В этом месте добавляем наши произвольные значения
			claims.Add(new Claim(ClaimTypes.Name, user.UserName));

			// Создадим и вернем токен
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
