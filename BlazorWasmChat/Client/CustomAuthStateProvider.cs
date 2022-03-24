using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorWasmChat.Client
{
	public class CustomAuthStateProvider : AuthenticationStateProvider
	{
		// Нам понадобятся LocalStorage и HttpClient
		ILocalStorageService _localStorage;
		HttpClient _httpClient;

		public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
		{
			_localStorage = localStorage;
			_httpClient = httpClient;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			// Получить токен из локального хранилища
			string token = await _localStorage.GetItemAsStringAsync("token");

			_httpClient.DefaultRequestHeaders.Authorization = null;

			var identity = new ClaimsIdentity();

			// Проверить токен
			if (IsTokenValid(token))
			{
				identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
			}

			// Создать пользователя и состояние входа
			var user = new ClaimsPrincipal(identity);

			var state = new AuthenticationState(user);

			// Обновить состояние авторизации
			// Будут перерисованы все поля Authorized
			NotifyAuthenticationStateChanged(Task.FromResult(state));

			return state;
		}

		// Вручную расшифровать токен и получить из него реквизиты
		public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			// Нам нужен 2-й элемент токена, payload
			var elements = jwt.Split('.');
			if (elements.Length < 3)
			{
				return Array.Empty<Claim>();
			}

			var payload = elements[1];

			// Это Base64 без дополняющих символов "=", добавить их при необходимости
			switch (payload.Length % 4)
			{
				case 2: payload += "=="; break;
				case 3: payload += "="; break;
			}

			// Раскодировать строку
			var data = Convert.FromBase64String(payload);

			// Преобразовать в словарь значений
			var kv = JsonSerializer.Deserialize<Dictionary<string, object>>(data);

			// Вернуть значения как список
			return kv!.Select(p => new Claim(p.Key, p.Value.ToString()!));
		}

		// Проверка корректности токена
		public bool IsTokenValid(string token)
		{
			if (token is null)
			{
				return false;
			}

			// Правильно составленный токен состоит из 3 частей, разделенных точками
			var elements = token.Split('.');
			if (elements.Length < 3)
			{
				return false;
			}

			return true;
		}
	}
}
