using AutoGestAPI.Database;
using AutoGestAPI.Middleware;
using AutoGestAPI.Models;
using AutoGestAPI.Services;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.ClientServices;
using AutoGestAPI.Services.OrderServices;
using AutoGestAPI.Services.ServiceServices;
using AutoGestAPI.Services.SingUpServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
var connection = builder.Environment.IsDevelopment() 
                    ? builder.Configuration["ConnectionStrings:Connection"]
                    : File.ReadAllText("/run/secrets/autogest-connection").Trim();
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

#region Rate Limit
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    //options.KnownProxies.Add(
    //    IPAddress.Parse("127.0.0.1")
    //);
    options.KnownProxies.Add(
        IPAddress.Parse("187.75.67.81")
    );
});

builder.Services.AddRateLimiter(options =>
{
    //options.AddTokenBucketLimiter("BruteForceProtection", opt =>
    //{
    //    opt.TokenLimit = 10;
    //    opt.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
    //    opt.TokensPerPeriod = 5;
    //    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    //    opt.QueueLimit = 0;
    //});

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext =>
        {
            var user = httpContext.User.Identity;
            if (user != null && user.IsAuthenticated)
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.FindFirst(ClaimTypes.PrimarySid)?.Value ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        QueueLimit = 0,
                        Window = TimeSpan.FromSeconds(30)
                    });

            return RateLimitPartition.GetTokenBucketLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 10,
                    TokensPerPeriod = 5,
                    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                });
        });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
#endregion

#region Add Services
builder.Services.AddScoped<AppDb>();
builder.Services.AddScoped<User>();
builder.Services.AddScoped<Client>();
builder.Services.AddScoped<Service>();
builder.Services.AddScoped<Order>();
builder.Services.AddScoped<OrderAndService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IServiceService,  ServiceService>();
builder.Services.AddScoped<IOrderService,  OrderService>();
#endregion


var app = builder.Build();


app.UseForwardedHeaders();

app.UseCors("AllowSites");

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapGet("/", () => "Api está funcionando!");

app.MapGet("/ping", () => StatusCodes.Status200OK);

app.MapGet("/db", async (AppDb db, HttpContext _) =>
{
    var inicio = Stopwatch.StartNew();

    await db.Database.OpenConnectionAsync();

    var abertura = inicio.Elapsed;

    var result = await db.Service
        .AsNoTracking()
        .Take(1)
        .ToListAsync();

    var query = inicio.Elapsed;

    await db.Database.CloseConnectionAsync();

    var total = inicio.Elapsed;

    Console.WriteLine(
        $"Conexão: {abertura.TotalMilliseconds:F2} ms | " +
        $"Query: {(query - abertura).TotalMilliseconds:F2} ms | " +
        $"Total: {total.TotalMilliseconds:F2} ms | "
    );
});

using (var scope = app.Services.CreateScope()) { // Adicionando Migrações
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();
