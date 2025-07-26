using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;

namespace DepartmentManagementApp.Application.Interfaces;

public interface IUserService
{
    UserResponse CreateUser(RegisterRequest request);

    UserResponse GetUserById(string id);

    UserResponse GetUserByEmail(string email);

    UserResponse GetUserByUserName(string fullName);

    UserResponse GetUserByPhoneNumber(string phoneNumber);

    IEnumerable<UserResponse> GetAllUsers();

    string ChangePassword(string email, string oldPassword, string newPassword);

    UserResponse UpdateUser(string id, RegisterRequest request);

    void DeleteUser(string id);
}