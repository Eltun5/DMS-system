using System.Text.Json.Serialization;
using WebApplication1.Domain.Enums;

namespace WebApplication1.Application.DTOs.Responses;

public record UserResponse(
    string Id,
    string FullName,
    string Email,
    int Age,
    string PhoneNumber,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Role Role,
    string Location,
    double Salary,
    string AdditionalInfo,
    bool IsActive,
    bool IsVerified,
    bool IsDeleted,
    DateTime NextTimeToChangePassword,
    DateTime CreatedAt,
    DateTime UpdatedAt);