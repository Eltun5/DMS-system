using AutoMapper;
using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;
using DepartmentManagementApp.Domain.Enums;
using DepartmentManagementApp.Domain.Models;
using DepartmentManagementApp.Infrastructure.Interfaces;
using Serilog;

namespace DepartmentManagementApp.Application.Service;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    
    private readonly IUserRepository _userRepository;

    private readonly IConfiguration _config;

    public UserService(IMapper mapper, IUserRepository userRepository, IConfiguration config)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _config = config;
    }

    public UserResponse CreateUser(RegisterRequest request)
    {
        Log.Information(_config["log:user:service:create:try"]!);
        VerifyCanCreateUserWithThisField(request);

        var user = InitializeUser(request);

        var response = _mapper.Map<UserResponse>(_userRepository.CreateUser(user));
        Log.Information(_config["log:user:service:create:success"]!);
        return response;
    }

    public UserResponse GetUserById(string id)
    {
        Log.Information(_config["log:user:service:get-by-id"]! + id);
        return _mapper.Map<UserResponse>(_userRepository.GetUserById(id));
    }

    public UserResponse GetUserByEmail(string email)
    {
        Log.Information(_config["log:user:service:get-by-email"]! + email);
        return _mapper.Map<UserResponse>(_userRepository.GetUserByEmail(email));
    }

    public UserResponse GetUserByUserName(string fullName)
    {
        Log.Information(_config["log:user:service:get-by-full-name"]! + fullName);
        return _mapper.Map<UserResponse>(_userRepository.GetUserByUserName(fullName));
    }

    public UserResponse GetUserByPhoneNumber(string phoneNumber)
    {
        Log.Information(_config["log:user:service:get-by-phone-number"]! + phoneNumber);
        return _mapper.Map<UserResponse>(_userRepository.GetUserByPhoneNumber(phoneNumber));
    }

    public IEnumerable<UserResponse> GetAllUsers()
    {
        Log.Information(_config["log:user:service:get-all"]!);
        return _mapper.Map<List<User>, List<UserResponse>>(_userRepository.GetAllUsers());
    }

    public string ChangePassword(string email, string oldPassword, string newPassword)
    {
        Log.Information(_config["log:user:service:change-password:try"]!);
        var user = _userRepository.GetUserByEmail(email);
        
        if (user == null)
        {
            Log.Information(_config["log:user:service:change-password:user-not-found-by-email"]! + email);
            throw new ArgumentException("User is not found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
        {
            Log.Information(_config["log:user:service:change-password:wrong-old-password"]!);
            throw new ArgumentException("Old password is wrong. Please enter correct password.");
        }
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.nextTimeToChangePassword = DateTime.Now.AddDays(30);
        user.UpdatedAt = DateTime.Now;
        
        return _config["log:user:service:change-password:success"]!;
    }

    public UserResponse UpdateUser(string id, RegisterRequest request)
    {
        Log.Information(_config["log:user:service:update:try"]!);
        
        return null;
    }

    public void DeleteUser(string id)
    {
        Log.Information(_config["log:user:service:delete"]! + id);
        var user = _userRepository.GetUserById(id);
        if (user != null)
        {
            user.IsDeleted = true;
            _userRepository.UpdateUser(user);
        }
    }

    private void VerifyCanCreateUserWithThisField(RegisterRequest request)
    {
        if (_userRepository.ExistsByEmail(request.Email))
            throw new ArgumentException(_config["log:user:service:create:email"]!);

        if (_userRepository.ExistsByFullName(request.FullName))
            throw new ArgumentException(_config["log:user:service:create:full-name"]!);

        if (_userRepository.ExistsByPhoneNumber(request.PhoneNumber))
            throw new ArgumentException(_config["log:user:service:create:phone-number"]!);
    }

    private User InitializeUser(RegisterRequest request)
    {
        User user = _mapper.Map<User>(request);
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.Now;
        user.IsActive = true;
        user.IsDeleted = false;
        user.IsVerified = false;
        user.Salary = 0;
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.nextTimeToChangePassword = DateTime.Now.AddDays(30);
        user.Role = Role.Employee;
        return user;
    }
}