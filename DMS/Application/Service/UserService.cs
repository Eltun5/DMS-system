using AutoMapper;
using Serilog;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;
using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.Interfaces;

namespace WebApplication1.Application.Service;

public class UserService(IMapper mapper,
    IUserRepository userRepository,
    IConfiguration config,
    IRedisService redisService)
    : IUserService
{
    public async Task<UserResponseWithDepartments> CreateUser(RegisterRequest request)
    {
        Log.Information(config["log:user:service:create:try"]!);
        VerifyCanCreateUserWithThisField(request);

        var user = InitializeUser(request);

        var response = mapper.Map<UserResponseWithDepartments>(await userRepository.CreateUser(user));
        Log.Information(config["log:user:service:create:success"]!);
        return response;
    }

    public async Task<UserResponseWithDepartments> GetUserById(string id)
    {
        Log.Information(config["log:user:service:get-by-id"]! + id);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserById(id));
    }

    public async Task<UserResponseWithDepartments> GetUserByEmail(string email)
    {
        Log.Information(config["log:user:service:get-by-email"]! + email);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserByEmail(email));
    }

    public async Task<UserResponseWithDepartments> GetUserByUserName(string fullName)
    {
        Log.Information(config["log:user:service:get-by-full-name"]! + fullName);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserByUserName(fullName));
    }

    public async Task<UserResponseWithDepartments> GetUserByPhoneNumber(string phoneNumber)
    {
        Log.Information(config["log:user:service:get-by-phone-number"]! + phoneNumber);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserByPhoneNumber(phoneNumber));
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetAllUsers()
    {
        Log.Information(config["log:user:service:get-all"]!);
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetAllUsers());
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetAllUsersByRole(string role)
    {
        Log.Information(config["log:user:service:get-all-roles"]!);
        Role role1 = (Role)Enum.Parse(typeof(Role), role);
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(
            await userRepository.GetAllUsersByRole(role1));
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetActiveUsers()
    {
        Log.Information(config["log:user:service:get-active-user"]!);
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetActiveUsers());
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetDeactivateUsers()
    {
        Log.Information(config["log:user:service:get-deactivated-user"]!);
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetDeactivateUsers());
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetDeletedUsers()
    {
        Log.Information(config["log:user:service:get-deleted-user"]!);
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetDeletedUsers());
    }

    public async Task<string> ChangePassword(ChangePasswordRequest request)
    {
        Log.Information(config["log:user:service:change-password:try"]!);
        var user = await userRepository.GetById(request.userId);
        
        if (user == null)
        {
            Log.Information(config["log:user:service:change-password:user-not-found-by-user-id"]! + request.userId);
            throw new ArgumentException("User is not found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.oldPassword, user.Password))
        {
            Log.Information(config["log:user:service:change-password:wrong-old-password"]!);
            throw new ArgumentException(config["log:user:service:change-password:wrong-old-password-exception"]!);
        }
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.newPassword);
        user.NextTimeToChangePassword = DateTime.Now.AddDays(30);
        user.UpdatedAt = DateTime.Now;
        
        await userRepository.UpdateUserPassword(user);
        
        return config["log:user:service:change-password:success"]!;
    }

    public async Task<UserResponseWithDepartments> AddDepartmentInUser(string userId, string departmentId)
    {
        Log.Information(config["log:user:service:add-department"]!);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.AddDepartmentInUser(userId, departmentId));
    }

    public async Task<UserResponseWithDepartments> RemoveDepartmentInUser(string userId, string departmentId)
    {
        Log.Information(config["log:user:service:remove-department"]!);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.RemoveDepartmentInUser(userId, departmentId));
    }

    public async Task<UserResponseWithDepartments> ChangeUserSalary(string userId, int salary)
    {
        Log.Information(config["log:user:service:change-user-salary"]!);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.ChangeUserSalary(userId, salary));
    }

    public async Task<UserResponseWithDepartments> ChangeUserRole(string userId, string role)
    {
        Log.Information(config["log:user:service:change-user-role"]!);
        Role role1 = (Role)Enum.Parse(typeof(Role), role);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.ChangeUserRole(userId, role1));
    }

    public async Task<UserResponseWithDepartments> DeactivateUser(string userId)
    {
        Log.Information(config["log:user:service:deactivate-user"]!);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.DeactivateUser(userId));
    }

    public async Task<UserResponseWithDepartments> ActivateUser(string userId)
    {
        Log.Information(config["log:user:service:activate-user"]!);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.ActivateUser(userId));
    }

    public async Task<UserResponseWithDepartments?> VerifyUser(string userId, string code)
    {
        Log.Information(config["log:user:service:verify-user"]!);
        
        var redisCode = await redisService.GetContentAsync(userId, "verification");

        if (code.Equals(redisCode))
        {
            return mapper.Map<UserResponseWithDepartments>(await userRepository.VerifyUser(userId));
        }
        return null;
    }

    public async Task<UserResponseWithDepartments> UpdateUser(string userId, UpdateUserRequest request)
    {
        Log.Information(config["log:user:service:update:try"]!);

        var user = await userRepository.GetById(userId);

        if (user == null) 
            throw new ArgumentException(config["log:user:service:update:user-not-found"]!);

        var updatedUser = MapRequestToUserForUpdate(user, request);

        updatedUser.UpdatedAt = DateTime.Now;
        var newUser = await userRepository.UpdateUser(updatedUser);
        
        Log.Information(config["log:user:service:update:success"]!);
        
        return mapper.Map<UserResponseWithDepartments>(newUser);
    }

    public async Task DeleteUser(string id)
    {
        Log.Information(config["log:user:service:delete"]! + id);
        var user = await userRepository.GetById(id);
        if (user != null)
        {
            user.IsDeleted = true;
            await userRepository.UpdateUser(user);
        }
    }

    private void VerifyCanCreateUserWithThisField(RegisterRequest request)
    {
        if (userRepository.ExistsByEmail(request.Email))
            throw new ArgumentException(config["log:user:service:create:email"]!);

        if (userRepository.ExistsByFullName(request.FullName))
            throw new ArgumentException(config["log:user:service:create:full-name"]!);

        if (userRepository.ExistsByPhoneNumber(request.PhoneNumber))
            throw new ArgumentException(config["log:user:service:create:phone-number"]!);
    }

    private User InitializeUser(RegisterRequest request)
    {
        User user = mapper.Map<User>(request);
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.Now;
        user.IsActive = true;
        user.IsDeleted = false;
        user.IsVerified = false;
        user.Salary = 0;
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.NextTimeToChangePassword = DateTime.Now.AddDays(30);
        user.Role = Role.Employee;
        return user;
    }

    private User MapRequestToUserForUpdate(User user, UpdateUserRequest request)
    {
        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Age = request.Age;
        user.PhoneNumber = request.PhoneNumber;
        user.Location = request.Location;
        user.AdditionalInfo = request.AdditionalInfo;

        return user;
    }
}