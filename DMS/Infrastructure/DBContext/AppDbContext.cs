using DepartmentManagementApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DepartmentManagementApp.Infrastructure.DBContext;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(user => user.Departments)
            .WithMany(department => department.Employees)
            .UsingEntity(j => j.ToTable("UsersDepartments")); 
    }
}