using AutoGestAPI.Database;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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
