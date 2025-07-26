using DepartmentManagementApp.Application.Interfaces;
using StackExchange.Redis;

namespace DepartmentManagementApp.Application.Service;

public class RedisTokenService : IRedisTokenService
{
    private readonly IDatabase _database;

    public RedisTokenService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task SetRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiresIn)
    {
        await _database.StringSetAsync($"refresh_token:{userId}", refreshToken, expiresIn);
    }

    public async Task<string?> GetRefreshTokenAsync(string userId)
    {
        return await _database.StringGetAsync($"refresh_token:{userId}");
    }

    public async Task DeleteRefreshTokenAsync(string userId)
    {
        await _database.KeyDeleteAsync($"refresh_token:{userId}");
    }
}