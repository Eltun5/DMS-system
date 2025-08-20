using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public record RegisterRequest(
    [property: Required]
    [property: StringLength(50, MinimumLength = 3)]
    [property: DefaultValue("John Doe")]
    [property: SwaggerSchema(Description = "User's full name")]
    string FullName = "John Doe",

    [property: Required]
    [property: EmailAddress]
    [property: DefaultValue("test@example.com")]
    [property: SwaggerSchema(Description = "User email")]
    string Email = "test@example.com",

    [property: Required]
    [property: StringLength(100, MinimumLength = 6)]
    [property: DefaultValue("P@ssw0rd123")]
    [property: SwaggerSchema(Description = "User password")]
    string Password = "P@ssw0rd123",

    [property: Range(1, 120)]
    [property: DefaultValue(25)]
    [property: SwaggerSchema(Description = "User age")]
    int Age = 25,

    [property: Required]
    [property: Phone]
    [property: DefaultValue("+994501234567")]
    [property: SwaggerSchema(Description = "User phone number")]
    string PhoneNumber = "+994501234567",

    [property: Required]
    [property: StringLength(100)]
    [property: DefaultValue("Baku, Azerbaijan")]
    [property: SwaggerSchema(Description = "User location")]
    string Location = "Baku, Azerbaijan",

    [property: StringLength(500)]
    [property: DefaultValue("Additional info about user")]
    [property: SwaggerSchema(Description = "Additional user info")]
    string AdditionalInfo = "Additional info about user"
);