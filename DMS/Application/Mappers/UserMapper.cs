using AutoMapper;
using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Domain.Models;

namespace DepartmentManagementApp.Application.Mappers;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<User, UserResponse>();
        
        CreateMap<RegisterRequest, User>();
    }
}