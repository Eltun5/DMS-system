using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;

public record LoginRequest(
    [property: Required]
    [property: EmailAddress]
    [property: DefaultValue("user@example.com")]
    [property: SwaggerSchema(Description = "User's email address")]
    string Email = "user@example.com",

    [property: Required]
    [property: MinLength(6)]
    [property: DefaultValue("Password123!")]
    [property: SwaggerSchema(Description = "User's password")]
    string Password = "Password123!"
);