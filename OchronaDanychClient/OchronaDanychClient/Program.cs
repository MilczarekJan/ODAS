using OchronaDanychClient.Components;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using OchronaDanychShared.Services;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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
/*
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.Run();
*/