namespace WebApplication1.Application.Interfaces;

public interface IUserPasswordHistoryService
{
    Task AddPasswordHistory(string userId, string password);
}