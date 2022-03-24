using BlazorWasmChat.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Добавим наш поставщик механизма авторизации
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Добавим авторизацию
builder.Services.AddAuthorizationCore();

// Добавим сервис доступа к локальному хранилищу браузера
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
