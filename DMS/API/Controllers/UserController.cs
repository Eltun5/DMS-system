using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.API.Controllers;

/// <summary>
/// API controller for managing users and their relationships with departments.
/// </summary>
[Route("api/v1/user")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user registration details.</param>
    /// <returns>The newly created user with department information.</returns>
    [HttpPost]
    public async Task<ActionResult<UserResponseWithDepartments>> CreateUser([FromBody] RegisterRequest request)
    {
        return await userService.CreateUser(request);
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user and their associated departments.</returns>
    [HttpGet("id")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserById([FromQuery] string id)
    {
        return await userService.GetUserById(id);
    }

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user and their associated departments.</returns>
    [HttpGet("email")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByEmail([FromQuery] string email)
    {
        return await userService.GetUserByEmail(email);
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user and their associated departments.</returns>
    [HttpGet("username")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByUsername([FromQuery] string username)
    {
        return await userService.GetUserByUserName(username);
    }

    /// <summary>
    /// Retrieves a user by their phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number of the user.</param>
    /// <returns>The user and their associated departments.</returns>
    [HttpGet("phone-number")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> GetUserByPhoneNumber([FromQuery] string phoneNumber)
    {
        return await userService.GetUserByPhoneNumber(phoneNumber);
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of users and their associated departments.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetUsers()
    {
        return Ok(await userService.GetAllUsers());
    }

    /// <summary>
    /// Retrieves users by their role.
    /// </summary>
    /// <param name="role">The role to filter users by.</param>
    /// <returns>A list of users matching the specified role.</returns>
    [HttpGet("role")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetUsersByRole([FromQuery] string role)
    {
        return Ok(await userService.GetAllUsersByRole(role));
    }

    /// <summary>
    /// Retrieves all active users.
    /// </summary>
    /// <returns>A list of active users.</returns>
    [HttpGet("active")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetActiveUsers()
    {
        return Ok(await userService.GetActiveUsers());
    }

    /// <summary>
    /// Retrieves all deactivated users.
    /// </summary>
    /// <returns>A list of deactivated users.</returns>
    [HttpGet("deactivate")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetDeactivateUsers()
    {
        return Ok(await userService.GetDeactivateUsers());
    }

    /// <summary>
    /// Retrieves all deleted users.
    /// </summary>
    /// <returns>A list of deleted users.</returns>
    [HttpGet("deleted")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseWithDepartments>>> GetDeletedUsers()
    {
        return Ok(await userService.GetDeletedUsers());
    }

    /// <summary>
    /// Changes the password of a user.
    /// </summary>
    /// <param name="request">The password change request containing current and new passwords.</param>
    /// <returns>A message indicating success or failure.</returns>
    [HttpPut("password")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        return await userService.ChangePassword(request);
    }

    /// <summary>
    /// Adds a department to a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="departmentId">The ID of the department to add.</param>
    /// <returns>The updated user with the new department.</returns>
    [HttpPatch("add-department")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<UserResponseWithDepartments>> AddDepartmentInUser(
        [FromQuery] string userId, 
        [FromQuery] string departmentId)
    {
        return await userService.AddDepartmentInUser(userId, departmentId);
    }

    /// <summary>
    /// Removes a department from a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="departmentId">The ID of the department to remove.</param>
    /// <returns>The updated user without the specified department.</returns>
    [HttpPatch("remove-department")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<UserResponseWithDepartments>> RemoveDepartmentInUser(
        [FromQuery] string userId, 
        [FromQuery] string departmentId)
    {
        return await userService.RemoveDepartmentInUser(userId, departmentId);
    }

    /// <summary>
    /// Changes a user's salary.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="salary">The new salary value.</param>
    /// <returns>The updated user with the new salary.</returns>
    [HttpPatch("change-salary")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<UserResponseWithDepartments>> ChangeSalary([FromQuery] string userId, [FromQuery] int salary)
    {
        return await userService.ChangeUserSalary(userId, salary);
    }

    /// <summary>
    /// Changes a user's role.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The new role to assign.</param>
    /// <returns>The updated user with the new role.</returns>
    [HttpPatch("change-role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseWithDepartments>> ChangeRole([FromQuery] string userId, [FromQuery] string role)
    {
        return await userService.ChangeUserRole(userId, role);
    }

    /// <summary>
    /// Deactivates a user.
    /// </summary>
    /// <param name="userId">The ID of the user to deactivate.</param>
    /// <returns>The updated user after deactivation.</returns>
    [HttpPatch("deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseWithDepartments>> DeactivateUser([FromQuery] string userId)
    {
        return await userService.DeactivateUser(userId);
    }

    /// <summary>
    /// Activates a user.
    /// </summary>
    /// <param name="userId">The ID of the user to activate.</param>
    /// <returns>The updated user after activation.</returns>
    [HttpPatch("activate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseWithDepartments>> ActivateUser([FromQuery] string userId)
    {
        return await userService.ActivateUser(userId);
    }

    /// <summary>
    /// Verifies a user's identity using a code.
    /// </summary>
    /// <param name="userId">The ID of the user to verify.</param>
    /// <param name="code">The verification code sent to the user.</param>
    /// <returns>The verified user.</returns>
    [HttpPatch("verify")]
    public async Task<ActionResult<UserResponseWithDepartments>> VerifyUser([FromQuery] string userId, [FromQuery] string code)
    {
        return await userService.VerifyUser(userId, code);
    }

    /// <summary>
    /// Updates user information.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="request">The updated user information.</param>
    /// <returns>The updated user.</returns>
    [HttpPut("update")]
    [Authorize]
    public async Task<ActionResult<UserResponseWithDepartments>> UpdateUser(
        [FromQuery] string userId,
        [FromBody] UpdateUserRequest request)
    {
        return await userService.UpdateUser(userId, request);
    }

    /// <summary>
    /// Permanently deletes a user.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    [HttpDelete("delete")]
    [Authorize(Roles = "Admin")]
    public async Task DeleteUser([FromQuery] string userId)
    {
        await userService.DeleteUser(userId);
    }
}
