using OchronaDanychClient.Components;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using OchronaDanychShared.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var uriBuilder = new UriBuilder("http://localhost:5093") //appSettingsSection.BaseAPIUrl
{
    //Path = appSettingsSection.BaseAPIUrl
};

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = uriBuilder.Uri);
builder.Services.AddScoped<Random>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
