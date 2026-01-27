using AutoGestAPI.Database;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.SingUpServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Cors Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSites", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});
#endregion

#region DB Configuration
var connection = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<AppDb>(options =>
{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});
#endregion

#region JWT Auth Configuration
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
#endregion

#region Add Services
builder.Services.AddScoped<AppDb>();
builder.Services.AddScoped<User>();
builder.Services.AddScoped<Client>();
builder.Services.AddScoped<Service>();
builder.Services.AddScoped<Order>();
builder.Services.AddScoped<OrderAndService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISingUpService, SingUpService>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
