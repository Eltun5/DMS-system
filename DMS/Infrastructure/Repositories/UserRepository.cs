using System.Linq.Expressions;
using DepartmentManagementApp.Domain.Enums;
using DepartmentManagementApp.Domain.Models;
using DepartmentManagementApp.Infrastructure.DBContext;
using DepartmentManagementApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;
    
    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> GetUserById(string id) =>
        await _dbContext.Users
            .Where(user => user.Id.ToString().Equals(id))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<User?> GetUserByEmail(string email) =>
        await _dbContext.Users
            .Where(user => user.Email.Equals(email))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<User?> GetUserByUserName(string fullName) =>
        await _dbContext.Users
            .Where(user => user.FullName.Equals(fullName))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<User?> GetUserByPhoneNumber(string phoneNumber) =>
        await _dbContext.Users
            .Where(user => user.PhoneNumber.Equals(phoneNumber))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<User>> GetAllUsers() => 
        await _dbContext.Users
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetAllUsersByRole(Role role) => 
        await _dbContext.Users
            .Where(user => user.Role.Equals(role))
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetActiveUsers() => 
        await _dbContext.Users
            .Where(user => user.IsActive)
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetDeactivateUsers() => 
        await _dbContext.Users
            .Where(user => !user.IsActive)
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetDeletedUsers() => 
        await _dbContext.Users
            .Where(user => user.IsDeleted)
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task UpdateUserPassword(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> AddDepartmentInUser(string userId, string departmentId)
    {
        var user = await GetById(userId);

        var department = await _dbContext.Departments.FindAsync(Guid.Parse(departmentId));

        if (user == null || department == null)
            return null;

        if (!user.Departments.Contains(department))
        {
            user.Departments.Add(department);
            await _dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> RemoveDepartmentInUser(string userId, string departmentId)
    {
        var user = await GetById(userId);

        var department = await _dbContext.Departments.FindAsync(Guid.Parse(departmentId));

        if (user == null || department == null) return null;

        if (user.Departments.Contains(department))
        {
            user.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> ChangeUserSalary(string userId, int salary)
    {
        var user = await GetById(userId);
        if (user == null) return null;
        if (!user.Salary.Equals(salary))
        {
            user.Salary = salary;
            await _dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> ChangeUserRole(string userId, Role role)
    {
        var user = await GetById(userId);
        if (user == null) return null;
        if (!user.Role.Equals(role))
        {
            user.Role = role;
            await _dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> DeactivateUser(string userId)
    {
        var user = await GetById(userId);
        if (user == null) return null;
        if (user.IsActive)
        {
            user.IsActive = false;
            await _dbContext.SaveChangesAsync(); 
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> ActivateUser(string userId)
    {
        var user = await GetById(userId);
        if (user == null) return null;
        if (!user.IsActive)
        {
            user.IsActive = true;
            await _dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> VerifyUser(string userId)
    {
        var user = await GetById(userId);
        if (user == null) return null;
        if (!user.IsVerified)
        {
            user.IsVerified = true;
            await _dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> UpdateUser(User user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
        return await GetUserByEmail(user.Email);
    }

    public async Task DeleteUser(string userId)
    {
        var user = await GetById(userId);
        if (user == null) return;
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public bool ExistsByEmail(string email) =>
        _dbContext.Users.Any(user => user.Email.ToLower() == email.ToLower());

    public bool ExistsByFullName(string fullName) =>
        _dbContext.Users.Any(user => user.FullName.ToLower() == fullName.ToLower());

    public bool ExistsByPhoneNumber(string phoneNumber) =>
        _dbContext.Users.Any(user => !user.IsDeleted && user.PhoneNumber.ToLower() == phoneNumber.ToLower());
    
    public async Task<User?> GetById(string userId) =>
        await _dbContext.Users
            .Include(u => u.Departments)
            .FirstOrDefaultAsync(u => u.Id.ToString().Equals(userId));
    
    private static Expression<Func<User, User>> Selector()
    {
        return user => new User
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Age = user.Age,
            PhoneNumber = user.PhoneNumber,
            Password = user.Password,
            Location = user.Location,
            Salary = user.Salary,
            Role = user.Role,
            AdditionalInfo = user.AdditionalInfo,
            IsActive = user.IsActive,
            IsDeleted = user.IsDeleted,
            IsVerified = user.IsVerified,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            NextTimeToChangePassword = user.NextTimeToChangePassword,
            LastPaidDate = user.LastPaidDate,
            LastLogin = user.LastLogin,
            Departments = user.Departments.Select(department => new Department
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName,
                Description = department.Description,
                ManagerId = department.ManagerId,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt
            }).ToList()
        };
    }
}