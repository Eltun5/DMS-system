using System.Text.Json.Serialization;
using WebApplication1.Domain.Enums;

namespace WebApplication1.Application.DTOs.Responses;

public record UserResponseWithDepartments(
    string Id,
    string FullName,
    string Email,
    int age,
    string PhoneNumber,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Role Role,
    string Location,
    double Salary,
    List<DepartmentResponse> Departments,
    string AdditionalInfo,
    bool IsActive,
    bool IsVerified,
    bool IsDeleted,
    DateTime nextTimeToChangePassword,
    DateTime CreatedAt,
    DateTime UpdatedAt);