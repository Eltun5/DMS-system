using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;

public record ChangePasswordRequest(
    [property: Required]
    [property: DefaultValue("123e4567-e89b-12d3-a456-426614174000")]
    [property: SwaggerSchema(Description = "ID of the user changing password")]
    string UserId = "123e4567-e89b-12d3-a456-426614174000",

    [property: Required]
    [property: StringLength(100, MinimumLength = 6)]
    [property: DefaultValue("OldPassword123!")]
    [property: SwaggerSchema(Description = "Current password of the user")]
    string OldPassword = "OldPassword123!",

    [property: Required]
    [property: StringLength(100, MinimumLength = 6)]
    [property: DefaultValue("NewPassword123!")]
    [property: SwaggerSchema(Description = "New password to set")]
    string NewPassword = "NewPassword123!"
    );