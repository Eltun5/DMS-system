using Serilog;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;
using WebApplication1.Infrastructure.DBContext;
using WebApplication1.Infrastructure.Interfaces;
using LoginRequest = WebApplication1.Application.DTOs.Requests.LoginRequest;

namespace WebApplication1.Application.Service;

public class AuthService(
    IUserRepository userRepository,
    IJwtService jwtService,
    IUserService userService,
    IConfiguration config,
    IRedisService redisService,
    AppDbContext dbContext)
    : IAuthService
{
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        Log.Information(config["log:auth:service:login"] + request.Email);

        var user = await userRepository.GetUserByEmail(request.Email);

        if (user == null)
        {
            Log.Information(config["log:auth:service:login:user-not-found-by-email"] + request.Email);
            throw new ArgumentException(config["log:auth:service:login:user-not-found-by-email"] + request.Email);
        }

        if (DateTime.Now.AddMonths(1) <= user.NextTimeToChangePassword)
        {
            Log.Information(config["log:auth:service:login:password-is-expired"]!);
            throw new ArgumentException(config["log:auth:service:login:password-is-expired"]!);
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            Log.Information(config["log:auth:service:login:wrong-password"]!);
            throw new ArgumentException(config["log:auth:service:login:wrong-password"]!);
        }

        if (user.IsDeleted || !user.IsActive || !user.IsVerified)
        {
            Log.Information(config["log:auth:service:login:invalid-user-status"]!);
            throw new ArgumentException(config["log:auth:service:login:invalid-user-status"]!);
        }

        var loginResponse = jwtService.GenerateToken(user);

        await StoreRefreshTokenAsync(user.Id.ToString(), loginResponse.RefreshToken);

        var newUser = await userRepository.GetById(user.Id.ToString());
        if (newUser == null) throw new BadHttpRequestException("User not found");
        newUser.LastLogin = DateTime.Now;
        dbContext.Update(newUser);
        await dbContext.SaveChangesAsync();
        
        return loginResponse;
    }

    public async Task<LoginResponse> RefreshToken(string refreshToken)
    {
        string? userId = jwtService.GetUserIdFromToken(refreshToken);

        Log.Verbose("userId : " + userId);
        
        if (userId != null && await ValidateRefreshTokenAsync(userId, refreshToken))
        {
            var user = await userRepository.GetUserById(userId);
            if (user != null)
            {
                var response = jwtService.GenerateToken(user);
                await DeleteRefreshTokenAsync(userId);
                await StoreRefreshTokenAsync(userId, response.RefreshToken);
                return response;
            }
        }

        throw new BadHttpRequestException("User not found!!!");
    }

    public async Task<UserResponseWithDepartments> Register(RegisterRequest request)
    {
        Log.Information(config["log:auth:service:register"] + request.Email);
        var userResponseWithDepartments = await userService.CreateUser(request);
        
        return userResponseWithDepartments;
    }

    public async Task Logout(string refreshToken)
    {
        string? userId = jwtService.GetUserIdFromToken(refreshToken);
        if (userId != null)
        {
            Log.Information(config["log:auth:service:logout"] + userId);
            await DeleteRefreshTokenAsync(userId);
        }
    }

    private async Task StoreRefreshTokenAsync(string userId, string token)
    {
        var expiresIn = TimeSpan.FromDays(7);

        await redisService.SetContentAsync(userId, "refresh_token",token, expiresIn);
    }

    private async Task<bool> ValidateRefreshTokenAsync(string userId, string providedToken)
    {
        var storedToken = await redisService.GetContentAsync(userId, "refresh_token");
        return storedToken == providedToken;
    }

    private async Task DeleteRefreshTokenAsync(string userId)
    {
        await redisService.DeleteContentAsync(userId, "refresh_token");
    }
}