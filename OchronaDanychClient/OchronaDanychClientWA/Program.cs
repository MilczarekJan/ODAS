using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using OchronaDanychShared.Services;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using OchronaDanychClientWA;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var uriBuilder = new UriBuilder("http://localhost:5093") //appSettingsSection.BaseAPIUrl
{
    //Path = appSettingsSection.BaseAPIUrl
};

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = uriBuilder.Uri);
builder.Services.AddScoped<Random>();
await builder.Build().RunAsync();