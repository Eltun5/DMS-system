using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;
using DepartmentManagementApp.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace DepartmentManagementApp.Application.Service;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    private readonly byte[] key;

    public JwtService(IConfiguration config)
    {
        _config = config;
        key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
    }

    public LoginResponse GenerateToken(User user)
    {
        return new LoginResponse(GenerateAccessToken(user),int.Parse(_config["Jwt:ExpiresInMinutes"]!), GenerateRefreshToken(user));
    }
    
    public string? getUserIdFromToken(string token)
    {
        var decodedPrincipal = DecodeJwtToken(token);

        if (decodedPrincipal != null)
        {
            var userId = decodedPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Log.Information(_config["log:Jwt:get-user-id"] + userId);

            return userId;
        }

        return null;
    }
    
    public string? getEmailFromToken(string token)
    {
        var decodedPrincipal = DecodeJwtToken(token);

        if (decodedPrincipal != null)
        {
            var email = decodedPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            Log.Information(_config["log:Jwt:get-email"] + email);

            return email;
        }

        return null;
    }

    private string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(this.key);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        Log.Information (_config["log:Jwt:access-token"]+ accessToken);

        return accessToken;
    }
    
    private string GenerateRefreshToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(this.key);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        
        var refreshToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        Log.Information(_config["log:Jwt:refresh-token"] + refreshToken);
        return refreshToken;
    }

    private ClaimsPrincipal? DecodeJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch (Exception ex)
        { 
            Log.Information(_config["log:Jwt:invalid-token"] + ex.Message);
            return null;
        }
    }
}