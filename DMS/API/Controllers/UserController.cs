using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.API.Controllers;

[Route("api/v1/user")]
[ApiController]
[SwaggerTag("API controller for managing users and their relationships with departments.")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Creates a new user.")]
    public async Task<ActionResult<UserResponseWithDepartments>> CreateUser([FromBody] RegisterRequest request)
    {
        return await userService.CreateUser(request);
    }

    [HttpGet("id")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves a user by their ID.")]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserById([FromQuery] string id)
    {
        return await userService.GetUserById(id);
    }
    
    [HttpGet("email")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves a user by their email address.")]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByEmail(
        [FromQuery, DefaultValue("example@gmail.com")]
        string email)
    {
        return await userService.GetUserByEmail(email);
    }

    [HttpGet("username")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves a user by their username.")]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByUsername([FromQuery, DefaultValue("Admin")] string username)
    {
        return await userService.GetUserByUserName(username);
    }

    [HttpGet("phone-number")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves a user by their phone number.")]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByPhoneNumber(
        [FromQuery, DefaultValue("+994 (70) 721 97 89")] 
        string phoneNumber)
    {
        return await userService.GetUserByPhoneNumber(phoneNumber);
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves all users.")]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetUsers()
    {
        return Ok(await userService.GetAllUsers());
    }

    [HttpGet("role")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves users by their role.")]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetUsersByRole(
        [FromQuery, DefaultValue("EMPLOYEE")] 
        string role)
    {
        return Ok(await userService.GetAllUsersByRole(role));
    }

    [HttpGet("active")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves all active users.")]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetActiveUsers()
    {
        return Ok(await userService.GetActiveUsers());
    }

    [HttpGet("deactivate")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves all deactivated users.")]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetDeactivateUsers()
    {
        return Ok(await userService.GetDeactivateUsers());
    }

    [HttpGet("deleted")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves all deleted users.")]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetDeletedUsers()
    {
        return Ok(await userService.GetDeletedUsers());
    }

    [HttpPut("password")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Changes the password of a user.")]
    public async Task<ActionResult<string>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        return await userService.ChangePassword(request);
    }

    [HttpPatch("add-department")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Adds a department to a user.")]
    public async Task<ActionResult<UserResponseWithDepartments>> AddDepartmentInUser(
        [FromQuery] string userId, 
        [FromQuery] string departmentId)
    {
        return await userService.AddDepartmentInUser(userId, departmentId);
    }

    [HttpPatch("remove-department")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Removes a department from a user.")]
    public async Task<ActionResult<UserResponseWithDepartments>> RemoveDepartmentInUser(
        [FromQuery] string userId, 
        [FromQuery] string departmentId)
    {
        return await userService.RemoveDepartmentInUser(userId, departmentId);
    }

    [HttpPatch("change-salary")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Changes a user's salary.")]
    public async Task<ActionResult<UserResponseWithDepartments>> ChangeSalary([FromQuery] string userId, [FromQuery] int salary)
    {
        return await userService.ChangeUserSalary(userId, salary);
    }

    [HttpPatch("change-role")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Changes a user's role.")]
    public async Task<ActionResult<UserResponseWithDepartments>> ChangeRole(
        [FromQuery] string userId,
        [FromQuery, DefaultValue("EMPLOYEE")] 
        string role)
    {
        return await userService.ChangeUserRole(userId, role);
    }

    [HttpPatch("deactivate")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Deactivates a user.")]
    public async Task<ActionResult<UserResponseWithDepartments>> DeactivateUser([FromQuery] string userId)
    {
        return await userService.DeactivateUser(userId);
    }
    
    [HttpPatch("activate")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Activates a user.")]
    public async Task<ActionResult<UserResponseWithDepartments>> ActivateUser([FromQuery] string userId)
    {
        return await userService.ActivateUser(userId);
    }

    [HttpGet("verify")]
    [SwaggerOperation(Summary = "Verifies a user's identity using a code.")]
    public async Task<ActionResult<UserResponseWithDepartments>> VerifyUser([FromQuery] string userId, [FromQuery] string code)
    {
        var user = await userService.VerifyUser(userId, code);
        return user==null ? NotFound() : user ;
    }

    [HttpPut("update")]
    [Authorize]
    [SwaggerOperation(Summary = "Updates user information.")]
    public async Task<ActionResult<UserResponseWithDepartments>> UpdateUser(
        [FromQuery] string userId,
        [FromBody] UpdateUserRequest request)
    {
        return await userService.UpdateUser(userId, request);
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Permanently deletes a user.")]
    public async Task DeleteUser([FromQuery] string userId)
    {
        await userService.DeleteUser(userId);
    }
}
