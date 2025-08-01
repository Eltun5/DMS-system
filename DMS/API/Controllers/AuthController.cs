using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.API.Controllers;

/// <summary>
/// Controller for handling authentication-related operations such as register, login, logout, and token refresh.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <returns>The registered user's information.</returns>
    [HttpPost("register")]
    public ActionResult<UserResponse> Register([FromBody] RegisterRequest request)
    {
        return Ok(authService.register(request));
    }

    /// <summary>
    /// Authenticates a user and returns an access token and refresh token.
    /// </summary>
    /// <param name="request">The login request containing credentials.</param>
    /// <returns>Login response with tokens and user details.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await authService.login(request);
        return Ok(response);
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to generate a new access token.</param>
    /// <returns>A new access token and refresh token.</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] string refreshToken)
    {
        var response = await authService.refreshToken(refreshToken);
        return Ok(response);
    }

    /// <summary>
    /// Logs out the user by invalidating the refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to invalidate.</param>
    [HttpPost("logout")]
    public async Task Logout([FromBody] string refreshToken)
    {
        await authService.logout(refreshToken);
    }
}
