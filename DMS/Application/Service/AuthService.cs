using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;
using DepartmentManagementApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;
using LoginRequest = DepartmentManagementApp.Application.DTOs.Requests.LoginRequest;

namespace DepartmentManagementApp.Application.Service;

public class AuthService : IAuthService
{
    private readonly IRedisTokenService _redisTokenService;

    private readonly IUserRepository _userRepository;

    private readonly IJwtService _jwtService;

    private readonly IUserService _userService;

    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IUserService userService,
        IConfiguration config, IRedisTokenService redisTokenService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _userService = userService;
        _config = config;
        _redisTokenService = redisTokenService;
    }

    public async Task<LoginResponse> login(LoginRequest request)
    {
        Log.Information(_config["log:auth:service:login"] + request.Email);

        var user = _userRepository.GetUserByEmail(request.Email);

        if (user == null)
        {
            Log.Information(_config["log:auth:service:login:user-not-found-by-email"] + request.Email);
            throw new ArgumentException(_config["log:auth:service:login:user-not-found-by-email"] + request.Email);
        }

        if (DateTime.Now.AddMonths(1) <= user.nextTimeToChangePassword)
        {
            Log.Information(_config["log:auth:service:login:password-is-expired"]!);
            throw new ArgumentException(_config["log:auth:service:login:password-is-expired"]!);
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            Log.Information(_config["log:auth:service:login:wrong-password"]!);
            throw new ArgumentException(_config["log:auth:service:login:wrong-password"]!);
        }

        var loginResponse = _jwtService.GenerateToken(user);

        await StoreRefreshTokenAsync(user.Id.ToString(), loginResponse.RefreshToken);

        return loginResponse;
    }

    public async Task<LoginResponse> refreshToken(string refreshToken)
    {
        string? userId = _jwtService.getUserIdFromToken(refreshToken);

        if (userId != null && await ValidateRefreshTokenAsync(userId, refreshToken))
        {
            var response = _jwtService.GenerateToken(_userRepository.GetUserById(userId));
            await DeleteRefreshTokenAsync(userId);
            await StoreRefreshTokenAsync(userId, response.RefreshToken);
            return response;
        }
        return null;
    }

    public UserResponse register(RegisterRequest request)
    {
        Log.Information(_config["log:auth:service:register"] + request.Email);
        return _userService.CreateUser(request);
    }

    public async Task logout(string refreshToken)
    {
        string? userId = _jwtService.getUserIdFromToken(refreshToken);
        if (userId != null)
        {
            Log.Information(_config["log:auth:service:logout"] + userId);
            await DeleteRefreshTokenAsync(userId);
        }
    }

    private async Task StoreRefreshTokenAsync(string userId, string token)
    {
        var expiresIn = TimeSpan.FromDays(7);

        await _redisTokenService.SetRefreshTokenAsync(userId, token, expiresIn);
    }

    private async Task<bool> ValidateRefreshTokenAsync(string userId, string providedToken)
    {
        var storedToken = await _redisTokenService.GetRefreshTokenAsync(userId);
        return storedToken == providedToken;
    }

    private async Task DeleteRefreshTokenAsync(string userId)
    {
        await _redisTokenService.DeleteRefreshTokenAsync(userId);
    }
}