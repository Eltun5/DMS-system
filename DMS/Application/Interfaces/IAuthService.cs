using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using LoginRequest = DepartmentManagementApp.Application.DTOs.Requests.LoginRequest;

namespace DepartmentManagementApp.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> login(LoginRequest request);
    
    Task<UserResponseWithDepartments> register(RegisterRequest request);
    
    Task<LoginResponse> refreshToken(string refreshToken);
    
    Task logout(string refreshToken);
}