using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Domain.Models;

namespace DepartmentManagementApp.Application.Interfaces;
    
public interface IJwtService
{
    LoginResponse GenerateToken(User user);
    
    string? GetUserIdFromToken(string token);
    
    string? GetEmailFromToken(string token);
}