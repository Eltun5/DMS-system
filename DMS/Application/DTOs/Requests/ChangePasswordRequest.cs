using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;

public record ChangePasswordRequest(
    [Required]
    [DefaultValue("123e4567-e89b-12d3-a456-426614174000")]
    [SwaggerSchema(Description = "ID of the user changing password")]
    string UserId = "123e4567-e89b-12d3-a456-426614174000",

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DefaultValue("OldPassword123!")]
    [SwaggerSchema(Description = "Current password of the user")]
    string OldPassword = "OldPassword123!",

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DefaultValue("NewPassword123!")]
    [SwaggerSchema(Description = "New password to set")]
    string NewPassword = "NewPassword123!"
);