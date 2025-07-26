using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentManagementApp.API.Controllers;

[Route("api/v1/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public UserResponse CreateUser(RegisterRequest request)
    {
        return _userService.CreateUser(request);
    }
    
    [HttpGet]
    [Authorize]
    public IEnumerable<UserResponse> GetUsers()
    {
        return _userService.GetAllUsers();
    }
    
    [HttpGet("/id/{id}")]
    public UserResponse GetUserById(string id)
    {
        return _userService.GetUserById(id);
    }

    [HttpGet("/email/{email}")]
    public UserResponse GetUserByEmail(string email)
    {
        return _userService.GetUserByEmail(email);
    }

    [HttpGet("/phone-number/{phoneNumber}")]
    public UserResponse GetUserByPhoneNumber(string phoneNumber)
    {
        return _userService.GetUserByPhoneNumber(phoneNumber);
    }

    [HttpPut("{id}")]
    public UserResponse UpdateUser(string id, RegisterRequest request)
    {
        return _userService.UpdateUser(id, request);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public void DeleteUser(string id)
    {
        _userService.DeleteUser(id);
    }
}