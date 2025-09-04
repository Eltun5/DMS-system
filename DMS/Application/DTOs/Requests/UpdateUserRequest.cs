using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;

public record UpdateUserRequest(
    [Required]
    [DefaultValue("John Doe")]
    [SwaggerSchema(Description = "Full name of the user")]
    string FullName = "John Doe",

    [Required]
    [EmailAddress]
    [DefaultValue("user@example.com")]
    [SwaggerSchema(Description = "User's email address")]
    string Email = "user@example.com",

    [Range(18, 100)]
    [DefaultValue(25)]
    [SwaggerSchema(Description = "Age of the user")]
    int Age = 25,

    [Required]
    [Phone]
    [DefaultValue("+994 (50) 123 45 67")]
    [SwaggerSchema(Description = "User phone number")]
    string PhoneNumber = "+994 (50) 123 45 67",

    [DefaultValue("Baku")]
    [SwaggerSchema(Description = "User's location")]
    string Location = "Baku",

    [DefaultValue("Additional information about the user")]
    [SwaggerSchema(Description = "Additional info about the user")]
    string AdditionalInfo = "Additional information about the user"
);