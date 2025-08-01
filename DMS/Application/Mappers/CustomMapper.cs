using AutoMapper;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Models;

namespace WebApplication1.Application.Mappers;

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