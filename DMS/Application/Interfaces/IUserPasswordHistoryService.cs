namespace DepartmentManagementApp.Application.Interfaces;

public interface IUserPasswordHistoryService
{
    Task AddPasswordHistory(string userId, string password);
}