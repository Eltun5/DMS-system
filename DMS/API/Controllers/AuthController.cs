using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentManagementApp.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public ActionResult<UserResponse> Register([FromBody] RegisterRequest request)
    {
        return Ok(_authService.register(request));
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.login(request);
        return  Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> RefreshToken(string refreshToken)
    {
        var response = await _authService.refreshToken(refreshToken);
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task Logout(string refreshToken)
    {
        await _authService.logout(refreshToken);
    }
}