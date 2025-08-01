using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.DBContext;
using WebApplication1.Infrastructure.Interfaces;

namespace WebApplication1.Infrastructure.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> CreateUser(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> GetUserById(string id) =>
        await dbContext.Users
            .Where(user => user.Id.ToString().Equals(id))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<User?> GetUserByEmail(string email) =>
        await dbContext.Users
            .Where(user => user.Email.Equals(email))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<User?> GetUserByUserName(string fullName) =>
        await dbContext.Users
            .Where(user => user.FullName.Equals(fullName))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<User?> GetUserByPhoneNumber(string phoneNumber) =>
        await dbContext.Users
            .Where(user => user.PhoneNumber.Equals(phoneNumber))
            .Include(user => user.Departments)
            .AsSplitQuery()
            .Select(Selector())
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<User>> GetAllUsers() => 
        await dbContext.Users
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetAllUsersByRole(Role role) => 
        await dbContext.Users
            .Where(user => user.Role.Equals(role))
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetActiveUsers() => 
        await dbContext.Users
            .Where(user => user.IsActive)
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetDeactivateUsers() => 
        await dbContext.Users
            .Where(user => !user.IsActive)
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task<IEnumerable<User>> GetDeletedUsers() => 
        await dbContext.Users
            .Where(user => user.IsDeleted)
            .Include(user => user.Departments)
            .AsSplitQuery() 
            .Select(Selector())
            .ToListAsync();

    public async Task UpdateUserPassword(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> AddDepartmentInUser(string userId, string departmentId)
    {
        var user = await GetById(userId);

        var department = await dbContext.Departments.FindAsync(Guid.Parse(departmentId));

        if (user == null || department == null)
            return null;

        if (!user.Departments.Contains(department))
        {
            user.Departments.Add(department);
            await dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> RemoveDepartmentInUser(string userId, string departmentId)
    {
        var user = await GetById(userId);

        var department = await dbContext.Departments.FindAsync(Guid.Parse(departmentId));

        if (user == null || department == null) return null;

        if (user.Departments.Contains(department))
        {
            user.Departments.Remove(department);
            await dbContext.SaveChangesAsync();
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
            await dbContext.SaveChangesAsync();
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
            await dbContext.SaveChangesAsync();
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
            await dbContext.SaveChangesAsync(); 
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
            await dbContext.SaveChangesAsync();
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
            await dbContext.SaveChangesAsync();
        }
        return await GetUserByEmail(user.Email);
    }

    public async Task<User?> UpdateUser(User user)
    {
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
        return await GetUserByEmail(user.Email);
    }

    public async Task DeleteUser(string userId)
    {
        var user = await GetById(userId);
        if (user == null) return;
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }

    public bool ExistsByEmail(string email) =>
        dbContext.Users.Any(user => user.Email.ToLower() == email.ToLower());

    public bool ExistsByFullName(string fullName) =>
        dbContext.Users.Any(user => user.FullName.ToLower() == fullName.ToLower());

    public bool ExistsByPhoneNumber(string phoneNumber) =>
        dbContext.Users.Any(user => !user.IsDeleted && user.PhoneNumber.ToLower() == phoneNumber.ToLower());
    
    public async Task<User?> GetById(string userId) =>
        await dbContext.Users
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