using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;
using WebApplication1.Application.Interfaces;
using WebApplication1.Application.Mappers;
using WebApplication1.Application.Service;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.DBContext;
using WebApplication1.Infrastructure.Interfaces;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.SignalR;
using Role = WebApplication1.Domain.Enums.Role;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => { }, typeof(CustomMapper).Assembly);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat-hub"))
                {
                    context.Token = accessToken;
                }
                
                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddHostedService<CustomBackgroundService>();

builder.Configuration.AddJsonFile("logMessages.json", optional: false, reloadOnChange: true);

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetSection("ConnectionStrings")["Redis"];
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, IdBasedUserIdProvider>();

var app = builder.Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/AppLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "api"); });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                FullName = "Admin",
                Email = "admin1234@gmail.com",
                Role = Role.Admin,
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Location = "Baku",
                Age = 18,
                Salary = 10000,
                PhoneNumber = "0551232323",
                AdditionalInfo = "Additional Info",
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                IsVerified = true,
                NextTimeToChangePassword = DateTime.Now.AddDays(30)
            });
            context.SaveChanges();
        }

        Log.Information("Database Initialized.");
    }
    catch (Exception ex)
    {
        Log.Information($"Initialization error: {ex.Message}");
    }
}

app.MapHub<ChatHub>("chat-hub");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();