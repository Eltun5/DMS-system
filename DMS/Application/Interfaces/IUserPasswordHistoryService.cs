namespace DepartmentManagementApp.Application.Interfaces;

public interface IUserPasswordHistoryService
{
    void AddPasswordHistory(string userId, string password);
}