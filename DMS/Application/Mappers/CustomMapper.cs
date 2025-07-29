using AutoMapper;
using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Domain.Models;

namespace DepartmentManagementApp.Application.Mappers;

public class CustomMapper: Profile
{
    public CustomMapper()
    {
        CreateMap<User, UserResponse>();
        
        CreateMap<User, UserResponseWithDepartments>();
        
        CreateMap<RegisterRequest, User>();
        
        CreateMap<Department, DepartmentResponse>();
        
        CreateMap<Department, DepartmentResponseWithUsers>();
    }
}