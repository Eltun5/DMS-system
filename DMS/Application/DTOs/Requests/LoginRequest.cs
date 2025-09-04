using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;

public record LoginRequest(
    [Required]
    [EmailAddress]
    [DefaultValue("user@example.com")]
    [SwaggerSchema(Description = "User's email address")]
    string Email = "user@example.com",

    [Required]
    [MinLength(6)]
    [DefaultValue("Password123!")]
    [SwaggerSchema(Description = "User's password")]
    string Password = "Password123!"
);