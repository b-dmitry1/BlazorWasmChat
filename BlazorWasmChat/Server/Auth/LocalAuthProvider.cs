using BlazorWasmChat.Shared;

namespace BlazorWasmChat.Server.Authorization
{
	// Поставщик локальной авторизации
	public class LocalAuthProvider : IChatAuthProvider
	{
		public bool TryAuthorize(UserLogin user)
		{
			// Если имя пользователя и пароль совпадают, то разрешить вход
			bool res = Chat.PasswordCorrect(user);

			return res;
		}
	}
}
