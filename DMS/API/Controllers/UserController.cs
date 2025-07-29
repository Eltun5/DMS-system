using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
    public async Task<ActionResult<UserResponseWithDepartments>> CreateUser([FromBody]RegisterRequest request)
    {
        return await _userService.CreateUser(request);
    }

    [HttpGet("id/{id}")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserById(string id)
    {
        return await _userService.GetUserById(id);
    }
    
    [HttpGet("email/{email}")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByEmail(string email)
    {
        return await _userService.GetUserByEmail(email);
    }

    [HttpGet("username/{username}")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByUsername(string username)
    {
        return await _userService.GetUserByUserName(username);
    }

    [HttpGet("phone-number/{phoneNumber}")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByPhoneNumber(string phoneNumber)
    {
        return await _userService.GetUserByPhoneNumber(phoneNumber);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetUsers()
    {
        return Ok(await _userService.GetAllUsers());
    }

    [HttpGet("role/{role}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetUsersByRole(string role)
    {
        return Ok(await _userService.GetAllUsersByRole(role));
    }

    [HttpGet("active")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetActiveUsers()
    {
        return Ok(await _userService.GetActiveUsers());
    }
    
    [HttpGet("deactivate")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetDeactivateUsers()
    {
        return Ok(await _userService.GetDeactivateUsers());
    }
    
    [HttpGet("deleted")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetDeletedUsers()
    {
        return Ok(await _userService.GetDeletedUsers());
    }
    
    [HttpPut("password")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> ChangePassword(
        [FromBody] ChangePasswordRequest request)
    {
        return await _userService.ChangePassword(request);
    }

    [HttpPut("add-department/{userId}/{departmentId}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<UserResponseWithDepartments>> AddDepartmentInUser(string userId, string departmentId)
    {
        return await _userService.AddDepartmentInUser(userId, departmentId);
    }
    
    [HttpPut("remove-department/{userId}/{departmentId}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<UserResponseWithDepartments>> RemoveDepartmentInUser(string userId, string departmentId)
    {
        return await _userService.RemoveDepartmentInUser(userId, departmentId);
    }

    [HttpPut("change-salary/{userId}/{salary}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<UserResponseWithDepartments>> ChangeSalary(string userId, int salary)
    {
        return await _userService.ChangeUserSalary(userId, salary);
    }

    [HttpPut("change-role/{userId}/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseWithDepartments>> ChangeRole(string userId, string role)
    {
        return await _userService.ChangeUserRole(userId, role);
    }

    [HttpPut("deactivate/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseWithDepartments>> DeactivateUser(string userId)
    {
        return await _userService.DeactivateUser(userId);
    }

    [HttpPut("activate/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseWithDepartments>> ActivateUser(string userId)
    {
        return await _userService.ActivateUser(userId);
    }
    
    [HttpPut("verify/{userId}/{code}")]
    public async Task<ActionResult<UserResponseWithDepartments>> VerifyUser(string userId,  string code)
    {
        return await _userService.VerifyUser(userId,  code);
    }

    [HttpPut("update/{userId}")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> UpdateUser(string userId,
        [FromBody] UpdateUserRequest request)
    {
        return await _userService.UpdateUser(userId, request);
    }

    [HttpPut("delete/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task DeleteUser(string userId)
    {
        await _userService.DeleteUser(userId);
    }
}