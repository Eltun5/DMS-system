using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using LoginRequest = WebApplication1.Application.DTOs.Requests.LoginRequest;

namespace WebApplication1.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> Login(LoginRequest request);
    
    Task<UserResponseWithDepartments> Register(RegisterRequest request);
    
    Task<LoginResponse> RefreshToken(string refreshToken);
    
    Task Logout(string refreshToken);
}