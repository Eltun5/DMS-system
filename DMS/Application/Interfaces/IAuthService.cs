using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using LoginRequest = WebApplication1.Application.DTOs.Requests.LoginRequest;

namespace WebApplication1.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> login(LoginRequest request);
    
    Task<UserResponseWithDepartments> register(RegisterRequest request);
    
    Task<LoginResponse> refreshToken(string refreshToken);
    
    Task logout(string refreshToken);
}