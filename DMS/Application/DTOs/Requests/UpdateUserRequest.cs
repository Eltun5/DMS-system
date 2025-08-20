using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;


public record UpdateUserRequest(
    [property: Required]
    [property: DefaultValue("John Doe")]
    [property: SwaggerSchema(Description = "Full name of the user")]
    string FullName = "John Doe",

    [property: Required]
    [property: EmailAddress]
    [property: DefaultValue("user@example.com")]
    [property: SwaggerSchema(Description = "User's email address")]
    string Email = "user@example.com",

    [property: Range(18, 100)]
    [property: DefaultValue(25)]
    [property: SwaggerSchema(Description = "Age of the user")]
    int Age = 25,

    [property: Required]
    [property: Phone]
    [property: DefaultValue("+994 (50) 123 45 67")]
    [property: SwaggerSchema(Description = "User phone number")]
    string PhoneNumber = "+994 (50) 123 45 67",

    [property: DefaultValue("Baku")]
    [property: SwaggerSchema(Description = "User's location")]
    string Location = "Baku",

    [property: DefaultValue("Additional information about the user")]
    [property: SwaggerSchema(Description = "Additional info about the user")]
    string AdditionalInfo = "Additional information about the user"
);