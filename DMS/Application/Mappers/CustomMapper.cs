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
        
        CreateMap<Department, DepartmentResponse>()
            .ConstructUsing(src => new DepartmentResponse(
                src.Id.ToString(),
                src.DepartmentName,
                src.Description ?? string.Empty,
                src.ManagerId.ToString(),
                src.IsActive,
                src.CreatedAt,
                src.UpdatedAt
            ));
        
        CreateMap<User, UserResponseWithDepartments>()
            .ConstructUsing((src, ctx) => new UserResponseWithDepartments(
                src.Id.ToString(),
                src.FullName,
                src.Email,
                src.Age,
                src.PhoneNumber,
                src.Role,
                src.Location ?? string.Empty,
                src.Salary,
                ctx.Mapper.Map<List<DepartmentResponse>>(src.Departments.ToList()),
                src.AdditionalInfo ?? string.Empty,
                src.IsActive,
                src.IsVerified,
                src.IsDeleted,
                src.NextTimeToChangePassword,
                src.CreatedAt,
                src.UpdatedAt ?? src.CreatedAt 
            ));
        
        CreateMap<Department, DepartmentResponseWithUsers>()
            .ConstructUsing((src, ctx) => new DepartmentResponseWithUsers(
                src.Id.ToString(),
                src.DepartmentName,
                src.Description ?? string.Empty,
                src.ManagerId.ToString(),
                ctx.Mapper.Map<List<UserResponse>>(src.Employees.ToList()),
                src.IsActive,
                src.CreatedAt,
                src.UpdatedAt
            ));
    }
}