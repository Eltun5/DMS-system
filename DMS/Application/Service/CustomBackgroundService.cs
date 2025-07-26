using DepartmentManagementApp.Domain.Models;
using DepartmentManagementApp.Infrastructure.DBContext;
using Serilog;

namespace DepartmentManagementApp.Application.Service;

public class CustomBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _config;

    public CustomBackgroundService(IServiceProvider serviceProvider, IConfiguration config)
    {
        _serviceProvider = serviceProvider;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Log.Information(_config["log:background-service:try"]!);
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            CleanPasswordDb(db);

            PayEmployeesSalary(db);

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private void CleanPasswordDb(AppDbContext db)
    {
        var oneYearAgo = DateTime.Now.AddYears(-1);

        var oldUserPasswordHistories =
            db.UserPasswordHistories.Where(uph => uph.ChangeTime < oneYearAgo).ToList();

        if (oldUserPasswordHistories.Any())
        {
            db.UserPasswordHistories.RemoveRange(oldUserPasswordHistories);
            db.SaveChangesAsync();
            Log.Information(_config["log:background-service:clean-password:success"]!);
        }
        else
        {
            Log.Information(_config["log:background-service:clean-password:nothing-to-clean"]!);
        }
    }

    private void PayEmployeesSalary(AppDbContext db)
    {
        var oneMonthAgo = DateTime.Now.AddMonths(-1);

        Log.Information(_config["log:background-service:pay-employees-salary"]!);;

        var employees = db.Users.Where
        (u => u.IsActive && !u.IsDeleted &&
              (u.LastLogin == null ? u.CreatedAt : u.LastLogin) < oneMonthAgo).ToList();

        if (employees.Any())
        {
            Log.Information(_config["log:background-service:pay-employees-salary"]! + employees.Count);
            foreach (User employee in employees)
            {
                employee.LastPaidDate = DateTime.Now;
            }

            db.SaveChangesAsync();
        }
    }
}