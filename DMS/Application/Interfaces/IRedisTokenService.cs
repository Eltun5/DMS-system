namespace DepartmentManagementApp.Application.Interfaces;

public interface IRedisTokenService
{
    Task SetRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiresIn);
    Task<string?> GetRefreshTokenAsync(string userId);
    Task DeleteRefreshTokenAsync(string userId);
}