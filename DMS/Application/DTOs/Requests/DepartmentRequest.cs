using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;

public record DepartmentRequest(
    [property: Required]
    [property: StringLength(100, MinimumLength = 2)]
    [property: DefaultValue("IT")]
    [property: SwaggerSchema(Description = "Name of the department")]
    string Name = "IT",

    [property: StringLength(500)]
    [property: DefaultValue("Handles all IT related tasks")]
    [property: SwaggerSchema(Description = "Description of the department")]
    string Description = "Handles all IT related tasks",

    [property: Required]
    [property: DefaultValue("123e4567-e89b-12d3-a456-426614174000")]
    [property: SwaggerSchema(Description = "ID of the department manager")]
    string ManagerId = "123e4567-e89b-12d3-a456-426614174000"
);
