using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.DBContext;
using WebApplication1.Domain.Enums;
using Serilog;

namespace WebApplication1.Extensions;

public static class DatabaseInitializer
{
    public static void Initialize(IServiceProvider services)
    {
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
            Log.Error($"Initialization error: {ex.Message}");
        }
    }
}