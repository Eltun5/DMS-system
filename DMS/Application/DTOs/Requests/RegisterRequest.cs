using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public record RegisterRequest(
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [DefaultValue("John Doe")]
    [SwaggerSchema(Description = "User's full name")]
    string FullName,

    [Required]
    [EmailAddress]
    [DefaultValue("test@example.com")]
    [SwaggerSchema(Description = "User email")]
    string Email,

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DefaultValue("P@ssw0rd123")]
    [SwaggerSchema(Description = "User password")]
    string Password,

    [Range(1, 120)]
    [DefaultValue(25)]
    [SwaggerSchema(Description = "User age")]
    int Age,

    [Required]
    [Phone]
    [DefaultValue("+994501234567")]
    [SwaggerSchema(Description = "User phone number")]
    string PhoneNumber,

    [Required]
    [StringLength(100)]
    [DefaultValue("Baku, Azerbaijan")]
    [SwaggerSchema(Description = "User location")]
    string Location,

    [StringLength(500)]
    [DefaultValue("Additional info about user")]
    [SwaggerSchema(Description = "Additional user info")]
    string AdditionalInfo
);