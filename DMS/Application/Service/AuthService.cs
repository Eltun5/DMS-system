using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;
using DepartmentManagementApp.Infrastructure.DBContext;
using DepartmentManagementApp.Infrastructure.Interfaces;
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

    private readonly AppDbContext _dbContext;
    
    public AuthService(IUserRepository userRepository, IJwtService jwtService, IUserService userService,
        IConfiguration config, IRedisTokenService redisTokenService,AppDbContext dbContext)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _userService = userService;
        _config = config;
        _redisTokenService = redisTokenService;
        _dbContext = dbContext;
    }

    public async Task<LoginResponse> login(LoginRequest request)
    {
        Log.Information(_config["log:auth:service:login"] + request.Email);

        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user == null)
        {
            Log.Information(_config["log:auth:service:login:user-not-found-by-email"] + request.Email);
            throw new ArgumentException(_config["log:auth:service:login:user-not-found-by-email"] + request.Email);
        }

        if (DateTime.Now.AddMonths(1) <= user.NextTimeToChangePassword)
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

        var newUser = await _userRepository.GetById(user.Id.ToString());
        if (newUser == null) throw new BadHttpRequestException("User not found");
        newUser.LastLogin = DateTime.Now;
        _dbContext.Update(newUser);
        await _dbContext.SaveChangesAsync();
        
        return loginResponse;
    }

    public async Task<LoginResponse> refreshToken(string refreshToken)
    {
        string? userId = _jwtService.GetUserIdFromToken(refreshToken);

        Log.Verbose("userId : " + userId);
        
        if (userId != null && await ValidateRefreshTokenAsync(userId, refreshToken))
        {
            var user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                var response = _jwtService.GenerateToken(user);
                await DeleteRefreshTokenAsync(userId);
                await StoreRefreshTokenAsync(userId, response.RefreshToken);
                return response;
            }
        }

        throw new BadHttpRequestException("User not found!!!");
    }

    public async Task<UserResponseWithDepartments> register(RegisterRequest request)
    {
        Log.Information(_config["log:auth:service:register"] + request.Email);
        return await _userService.CreateUser(request);
    }

    public async Task logout(string refreshToken)
    {
        string? userId = _jwtService.GetUserIdFromToken(refreshToken);
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