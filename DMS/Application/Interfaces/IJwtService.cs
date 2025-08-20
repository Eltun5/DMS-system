using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Models;

namespace WebApplication1.Application.Interfaces;
    
public interface IJwtService
{
    LoginResponse GenerateToken(User user);
    
    string? GetUserIdFromToken(string token);
}