using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
[SwaggerTag("Controller for handling authentication-related operations such as register, login, logout, and token refresh.")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Registers a new user.")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
    {
        return Ok(await authService.Register(request));
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Authenticates a user and returns an access token and refresh token.")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await authService.Login(request);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refreshes the access token using a valid refresh token.")]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] string refreshToken)
    {
        var response = await authService.RefreshToken(refreshToken);
        return Ok(response);
    }

    [HttpPost("logout")]
    [SwaggerOperation(Summary = "Logs out the user by invalidating the refresh token.")]
    public async Task Logout([FromBody] string refreshToken)
    {
        await authService.Logout(refreshToken);
    }
}
