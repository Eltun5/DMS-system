using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Domain.Models;

namespace DepartmentManagementApp.Application.Interfaces;
    
public interface IJwtService
{
    LoginResponse GenerateToken(User user);
    
    string? getUserIdFromToken(string token);
    
    string? getEmailFromToken(string token);
}