using System.DirectoryServices;

namespace BlazorWasmChat.Server.Authorization
{
	public class AuthActiveDirectory
	{
		public static bool IsUserInActiveDirectory(string username, string password)
		{
			bool res = false;

			try
			{
				string domainName = username.Substring(0, username.IndexOf('\\'));

				username = username.Substring(domainName.Length + 1);

				var direenty = new DirectoryEntry("LDAP://" + domainName, username, password);
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
