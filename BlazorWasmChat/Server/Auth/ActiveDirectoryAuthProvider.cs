using BlazorWasmChat.Shared;
using System.DirectoryServices;

namespace BlazorWasmChat.Server.Authorization
{
	// Поставщик авторизации в ActiveDirectory
	public class ActiveDirectoryAuthProvider : IChatAuthProvider
	{
		// Мы знаем, что подходит только для Windows, поэтому уберем предупреждающее сообщение
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
		public bool TryAuthorize(UserLogin user)
		{
			// Проверить входные данные
			if (user.UserName is null || user.Password is null)
			{
				return false;
			}

			// Имя пользователя должно быть в формате домен\пользователь
			if (!user.UserName.Contains('\\'))
			{
				return false;
			}

			bool res = false;

			try
			{
				// Извлечь из полного имени пользователя домен
				var domainName = user.UserName.Substring(0, user.UserName.IndexOf('\\'));
				// и имя пользователя
				var username = user.UserName.Substring(domainName.Length + 1);

				// Попробовать подключиться и прочитать каталог
				var direenty = new DirectoryEntry("LDAP://" + domainName, username, user.Password);
				var search = new DirectorySearcher(direenty);
				var results = search.FindOne();
				
				res = true;
			}
			catch
			{
			}

			return res;
		}
	}
}
