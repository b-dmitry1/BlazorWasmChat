using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorWasmChat.Client
{
	public class CustomAuthStateProvider : AuthenticationStateProvider
	{
		ILocalStorageService _localStorage;
		HttpClient _httpClient;

		public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
		{
			_localStorage = localStorage;
			_httpClient = httpClient;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await _localStorage.GetItemAsStringAsync("token");

			_httpClient.DefaultRequestHeaders.Authorization = null;

			var identity = new ClaimsIdentity();

			if (!string.IsNullOrEmpty(token))
			{
				identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
			}

			var user = new ClaimsPrincipal(identity);

			var state = new AuthenticationState(user);

			NotifyAuthenticationStateChanged(Task.FromResult(state));

			return state;
		}

		public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			var elements = jwt.Split('.');
			if (elements.Length < 3)
			{
				return Array.Empty<Claim>();
			}

			var payload = elements[1];

			switch (payload.Length % 4)
			{
				case 2: payload += "=="; break;
				case 3: payload += "="; break;
			}

			var data = Convert.FromBase64String(payload);

			var kv = JsonSerializer.Deserialize<Dictionary<string, object>>(data);

			return kv!.Select(p => new Claim(p.Key, p.Value.ToString()!));
		}
	}
}
