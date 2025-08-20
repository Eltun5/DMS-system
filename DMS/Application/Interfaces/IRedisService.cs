namespace WebApplication1.Application.Interfaces;

public interface IRedisService
{
    Task SetContentAsync(string userId,string title, string content, TimeSpan expiresIn);
    
    Task<string?> GetContentAsync(string userId, string title);
    
    Task DeleteContentAsync(string userId, string title);
}