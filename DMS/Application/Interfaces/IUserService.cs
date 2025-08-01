using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;

namespace WebApplication1.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseWithDepartments> CreateUser(RegisterRequest request);

    Task<UserResponseWithDepartments> GetUserById(string id);

    Task<UserResponseWithDepartments> GetUserByEmail(string email);

    Task<UserResponseWithDepartments> GetUserByUserName(string fullName);

    Task<UserResponseWithDepartments> GetUserByPhoneNumber(string phoneNumber);

    Task<IEnumerable<UserResponseWithDepartments>> GetAllUsers();
    
    Task<IEnumerable<UserResponseWithDepartments>> GetAllUsersByRole(string role);
    
    Task<IEnumerable<UserResponseWithDepartments>> GetActiveUsers();
    
    Task<IEnumerable<UserResponseWithDepartments>> GetDeactivateUsers();
    
    Task<IEnumerable<UserResponseWithDepartments>> GetDeletedUsers();
    
    Task<string> ChangePassword(ChangePasswordRequest request);

    Task<UserResponseWithDepartments> AddDepartmentInUser(string userId, string departmentId);
    
    Task<UserResponseWithDepartments> RemoveDepartmentInUser(string userId, string departmentId);
    
    Task<UserResponseWithDepartments> ChangeUserSalary(string userId, int salary);
    
    Task<UserResponseWithDepartments> ChangeUserRole(string userId, string role);
    
    Task<UserResponseWithDepartments> DeactivateUser(string userId);
    
    Task<UserResponseWithDepartments> ActivateUser(string userId);
    
    Task<UserResponseWithDepartments> VerifyUser(string userId, string code);
    
    Task<UserResponseWithDepartments> UpdateUser(string userId, UpdateUserRequest request);

    Task DeleteUser(string userId);
}