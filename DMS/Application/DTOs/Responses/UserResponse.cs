using System.Text.Json.Serialization;
using DepartmentManagementApp.Domain.Enums;

namespace DepartmentManagementApp.Application.DTOs.Responses;

public record UserResponse(
    string Id,
    string FullName,
    string PhoneNumber,
    string Email,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Role Role,
    string Location,
    double Salary,
    string AdditionalInfo,
    DateTime CreatedAt,
    DateTime UpdatedAt);