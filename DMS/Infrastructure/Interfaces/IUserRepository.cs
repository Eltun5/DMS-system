using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Models;

namespace WebApplication1.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<User?> CreateUser(User user);

    Task<User?> GetUserById(string id);

    Task<User?> GetUserByEmail(string email);

    Task<User?> GetUserByUserName(string fullName);

    Task<User?> GetUserByPhoneNumber(string phoneNumber);

    Task<IEnumerable<User>> GetAllUsers();
    
    Task<IEnumerable<User>> GetAllUsersByRole(Role role);
    
    Task<IEnumerable<User>> GetActiveUsers();
    
    Task<IEnumerable<User>> GetDeactivateUsers();
    
    Task<IEnumerable<User>> GetDeletedUsers();
    
    Task UpdateUserPassword(User user);

    Task<User?> AddDepartmentInUser(string userId, string departmentId);
    
    Task<User?> RemoveDepartmentInUser(string userId, string departmentId);
    
    Task<User?> ChangeUserSalary(string userId, int salary);
    
    Task<User?> ChangeUserRole(string userId, Role role);
    
    Task<User?> DeactivateUser(string userId);
    
    Task<User?> ActivateUser(string userId);
    
    Task<User?> VerifyUser(string userId);
    
    Task<User?> UpdateUser(User user);

    Task DeleteUser(string userId);
    
    Task<User?> GetById(string userId);
    
    bool ExistsByEmail(string email);

    bool ExistsByFullName(string fullName);

    bool ExistsByPhoneNumber(string phoneNumber);
}