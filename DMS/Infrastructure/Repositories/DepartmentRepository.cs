using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.DBContext;
using WebApplication1.Infrastructure.Interfaces;

namespace WebApplication1.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    public async Task CreateDepartment(Department department)
    {
        context.Departments.Add(department);
        await context.SaveChangesAsync();
    }

    public async Task<Department?> GetDepartmentById(string id) => 
        await context.Departments
            .Where(department => department.Id.ToString().Equals(id))
            .Include(department => department.Employees)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<Department?> GetDepartmentByName(string name) => 
        await context.Departments
            .Where(department => department.DepartmentName.Equals(name))
            .Include(department => department.Employees)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Department>> GetAllDepartments() =>
        await context.Departments
            .Include(department => department.Employees)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<Department>> GetActiveDepartments() =>
        await context.Departments
            .Where(department => department.IsActive)
            .Include(department => department.Employees)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();
    public async Task UpdateDepartment(Department department)
    {
        context.Departments.Update(department);
        await context.SaveChangesAsync();
    }

    public async Task<Department?> AddEmployeeInDepartment(string departmentId, string employeeId)
    {
        Department? department = await GetById(departmentId);

        User? user = await context.Users.FindAsync(new Guid(employeeId));
        if (department == null || user == null)
        {
            Log.Information("Cannot found department or user.");
            return null;
        }

        if (!department.Employees.Contains(user))
        {
            department.Employees.Add(user);
            await context.SaveChangesAsync();
        }
        return await GetDepartmentById(departmentId);
    }

    public async Task<Department?> RemoveEmployeeFromDepartment(string departmentId, string employeeId)
    {
        Department? department = await GetById(departmentId);

        User? user = await context.Users.FindAsync(new Guid(employeeId));
        if (department == null || user == null)
        {
            Log.Information("Cannot found department or user.");
            return null;
        }

        if (department.Employees.Contains(user))
        {
            department.Employees.Remove(user);
            await context.SaveChangesAsync();
        }
        return await GetDepartmentById(departmentId);
    }

    public async Task DeactivateDepartment(string id)
    {
        var department = await GetById(id);
        if (department != null) department.IsActive = false;
        await context.SaveChangesAsync();
    }

    public async Task ActivateDepartment(string id)
    {
        var department = await GetById(id);
        if (department != null) department.IsActive = true;
        await context.SaveChangesAsync();
    }

    private async Task<Department?> GetById(string id) =>
        await context.Departments
            .Include(department => department.Employees)
            .FirstOrDefaultAsync(department => department.Id.ToString().Equals(id));
    
    public bool ExistsByName(string name) =>
        context.Departments.Any(department => department.DepartmentName.ToLower()==name);
    
    private static Expression<Func<Department, Department>> Selector()
    {
        return department => new Department()
        {
            Id = department.Id,
            DepartmentName = department.DepartmentName,
            Description = department.Description,
            ManagerId = department.ManagerId,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt,
            Employees = department.Employees.Select(user => new User()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                Location = user.Location,
                Salary = user.Salary,
                Role = user.Role,
                AdditionalInfo = user.AdditionalInfo,
                IsActive = user.IsActive,
                IsVerified = user.IsVerified,
                IsDeleted = user.IsDeleted,
                NextTimeToChangePassword = user.NextTimeToChangePassword,
                LastPaidDate = user.LastPaidDate,
                UpdatedAt = user.UpdatedAt,
                DeleteAt = user.DeleteAt
            }).ToList()
        };
    }
}