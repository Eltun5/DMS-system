using StackExchange.Redis;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.Application.Service;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;

    public RedisService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task SetContentAsync(string userId, string title , string content, TimeSpan expiresIn)
    {
        await _database.StringSetAsync($"{title}:{userId}", content, expiresIn);
    }

    public async Task<string?> GetContentAsync(string userId, string title)
    {
        return await _database.StringGetAsync($"{title}:{userId}");
    }

    public async Task DeleteContentAsync(string userId, string title)
    {
        await _database.KeyDeleteAsync($"{title}:{userId}");
    }
}