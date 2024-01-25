using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using OchronaDanychAPI.Models;
using OchronaDanychAPI.Services.AuthService;
using OchronaDanychAPI.Services.TransferService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Microsoft.EntityFrameworkCore.SqlServer
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITransferService, TransferService>();
/*
// Configure app settings
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Register the configuration
builder.Services.AddSingleton<IConfiguration>(configuration);
*/
string token = builder.Configuration.GetSection("AppSettings:Token").Value;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        // options.Authority = "https://localhost:5001";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token)),
            ValidateIssuerSigningKey = true,
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsePolicy", builder =>
    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5127")); //.WithOrigins("http://localhost:5127")
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("MyCorsePolicy", builder =>
//    builder.AllowAnyHeader().AllowAnyHeader().WithOrigins("https://mySite.pl"));
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("MyCorsePolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
