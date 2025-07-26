using DepartmentManagementApp.Domain.Models;

namespace DepartmentManagementApp.Infrastructure.Interfaces;

public interface IUserRepository
{
    User CreateUser(User user);
    
    User GetUserById(string id);
    
    User GetUserByEmail(string email);
    
    User GetUserByUserName(string userName);
    
    User GetUserByPhoneNumber(string phoneNumber);
    
    List<User> GetAllUsers();
    
    void UpdateUser(User user);

    public bool ExistsByEmail(string email);
    
    public bool ExistsByFullName(string fullName);

    public bool ExistsByPhoneNumber(string phoneNumber);
}