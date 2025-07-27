using DepartmentManagementApp.Domain.Models;
using DepartmentManagementApp.Infrastructure.DBContext;
using DepartmentManagementApp.Infrastructure.Interfaces;

namespace DepartmentManagementApp.Infrastructure.Repositories;

public class SqlUserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public SqlUserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return user;
    }

    public User GetUserById(string id) => 
        _dbContext.Users.FirstOrDefault(user => user.Id.ToString().Equals(id));

    public User GetUserByEmail(string email) =>
        _dbContext.Users.FirstOrDefault(user => user.Email.Equals(email));

    public User GetUserByUserName(string fullName) =>
        _dbContext.Users.FirstOrDefault(user => user.FullName.Equals(fullName));

    public User GetUserByPhoneNumber(string phoneNumber) =>
        _dbContext.Users.FirstOrDefault(user => user.PhoneNumber.Equals(phoneNumber));

    public List<User> GetAllUsers() => _dbContext.Users.ToList();

    public void UpdateUser(User user)
    {
        _dbContext.Users.Update(user);
    }

    public bool ExistsByEmail(string email) =>
        _dbContext.Users.Any(user => user.Email.ToLower() == email.ToLower());

    public bool ExistsByFullName(string fullName) =>
        _dbContext.Users.Any(user => user.FullName.ToLower() == fullName.ToLower());

    public bool ExistsByPhoneNumber(string phoneNumber) =>
        _dbContext.Users.Any(user => !user.IsDeleted && user.PhoneNumber.ToLower() == phoneNumber.ToLower());
}