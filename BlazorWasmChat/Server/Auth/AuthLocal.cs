using System.DirectoryServices;

namespace BlazorWasmChat.Server.Authorization
{
	public class AuthLocal
	{
		public static bool TryAuthLocal(string username, string password)
		{
			bool res = Chat.Users.Exists(x => x.UserName == username && x.Password == password);

			return res;
		}
	}
}
