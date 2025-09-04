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
    IRedisService redisService,
    IEmailService emailService)
    : IUserService
{
    public async Task<UserResponseWithDepartments> CreateUser(RegisterRequest request)
    {
        VerifyCanCreateUserWithThisField(request);

        var user = InitializeUser(request);
        
        var response = mapper.Map<UserResponseWithDepartments>(await userRepository.CreateUser(user));
        
        string random = Guid.NewGuid().ToString();
        
        await redisService.SetContentAsync(response.Id,
            "verification", random, TimeSpan.FromDays(1));
        
        await emailService.SendEmailAsync(request.Email, "Verification DMS", 
            "https://localhost:7035/api/v1/user/verify?userId=" +
            response.Id +
            "&code=" + random);
        
        return response;
    }

    public async Task<UserResponseWithDepartments> GetUserById(string id)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserById(id));
    }

    public async Task<UserResponseWithDepartments> GetUserByEmail(string email)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserByEmail(email));
    }

    public async Task<UserResponseWithDepartments> GetUserByUserName(string fullName)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserByUserName(fullName));
    }

    public async Task<UserResponseWithDepartments> GetUserByPhoneNumber(string phoneNumber)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.GetUserByPhoneNumber(phoneNumber));
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetAllUsers()
    {
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetAllUsers());
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetAllUsersByRole(string role)
    {
        Role role1 = (Role)Enum.Parse(typeof(Role), role);
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(
            await userRepository.GetAllUsersByRole(role1));
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetActiveUsers()
    {
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetActiveUsers());
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetDeactivateUsers()
    {
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetDeactivateUsers());
    }

    public async Task<IEnumerable<UserResponseWithDepartments>> GetDeletedUsers()
    {
        return mapper.Map<IEnumerable<User>, IEnumerable<UserResponseWithDepartments>>(await userRepository.GetDeletedUsers());
    }

    public async Task<string> ChangePassword(ChangePasswordRequest request)
    {
        var user = await userRepository.GetById(request.UserId);
        
        if (user == null)
        {
            Log.Information(config["log:user:service:change-password:user-not-found-by-user-id"]! + request.UserId);
            throw new ArgumentException("User is not found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
        {
            Log.Information(config["log:user:service:change-password:wrong-old-password"]!);
            throw new ArgumentException(config["log:user:service:change-password:wrong-old-password-exception"]!);
        }
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.NextTimeToChangePassword = DateTime.Now.AddDays(30);
        user.UpdatedAt = DateTime.Now;
        
        await userRepository.UpdateUserPassword(user);
        
        return config["log:user:service:change-password:success"]!;
    }

    public async Task<UserResponseWithDepartments> AddDepartmentInUser(string userId, string departmentId)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.AddDepartmentInUser(userId, departmentId));
    }

    public async Task<UserResponseWithDepartments> RemoveDepartmentInUser(string userId, string departmentId)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.RemoveDepartmentInUser(userId, departmentId));
    }

    public async Task<UserResponseWithDepartments> ChangeUserSalary(string userId, int salary)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.ChangeUserSalary(userId, salary));
    }

    public async Task<UserResponseWithDepartments> ChangeUserRole(string userId, string role)
    {
        Role role1 = (Role)Enum.Parse(typeof(Role), role);
        return mapper.Map<UserResponseWithDepartments>(await userRepository.ChangeUserRole(userId, role1));
    }

    public async Task<UserResponseWithDepartments> DeactivateUser(string userId)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.DeactivateUser(userId));
    }

    public async Task<UserResponseWithDepartments> ActivateUser(string userId)
    {
        return mapper.Map<UserResponseWithDepartments>(await userRepository.ActivateUser(userId));
    }

    public async Task<UserResponseWithDepartments?> VerifyUser(string userId, string code)
    {
        var redisCode = await redisService.GetContentAsync(userId, "verification");

        if (code.Equals(redisCode))
        {
            return mapper.Map<UserResponseWithDepartments>(await userRepository.VerifyUser(userId));
        }
        return null;
    }

    public async Task<UserResponseWithDepartments> UpdateUser(string userId, UpdateUserRequest request)
    {
        var user = await userRepository.GetById(userId);

        if (user == null) 
            throw new ArgumentException(config["log:user:service:update:user-not-found"]!);

        var updatedUser = MapRequestToUserForUpdate(user, request);

        updatedUser.UpdatedAt = DateTime.Now;
        var newUser = await userRepository.UpdateUser(updatedUser);
        
        return mapper.Map<UserResponseWithDepartments>(newUser);
    }

    public async Task DeleteUser(string id)
    {
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
        {
            Log.Information(config["log:user:service:create:exist:email"]!);
            throw new ApplicationException(config["log:user:service:create:exist:email"]!);
        }


        if (userRepository.ExistsByFullName(request.FullName))
        {
            Log.Information(config["log:user:service:create:exist:full-name"]!);
            throw new ApplicationException(config["log:user:service:create:exist:full-name"]!);

        }

        if (userRepository.ExistsByPhoneNumber(request.PhoneNumber)){
            Log.Information(config["log:user:service:create:exist:email"]!);
            throw new ApplicationException(config["log:user:service:create:exist:email"]!);
        }
    }

    private User InitializeUser(RegisterRequest request)
    {
        User user = new User();
        user.Id = Guid.NewGuid();
        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.Age = request.Age;
        user.PhoneNumber = request.PhoneNumber;
        user.Location = request.Location;
        user.AdditionalInfo = request.AdditionalInfo;
        user.CreatedAt = DateTime.Now;
        user.IsActive = true;
        user.IsDeleted = false;
        user.IsVerified = false;
        user.Salary = 0;
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