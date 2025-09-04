using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Application.DTOs.Requests;

public record DepartmentRequest(
    [Required]
    [StringLength(100, MinimumLength = 2)]
    [DefaultValue("IT")]
    [SwaggerSchema(Description = "Name of the department")]
    string Name = "IT",

    [StringLength(500)]
    [DefaultValue("Handles all IT related tasks")]
    [SwaggerSchema(Description = "Description of the department")]
    string Description = "Handles all IT related tasks",

    [Required]
    [DefaultValue("123e4567-e89b-12d3-a456-426614174000")]
    [SwaggerSchema(Description = "ID of the department manager")]
    string ManagerId = "123e4567-e89b-12d3-a456-426614174000"
);

