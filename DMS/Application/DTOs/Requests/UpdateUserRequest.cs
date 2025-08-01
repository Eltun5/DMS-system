namespace WebApplication1.Application.DTOs.Requests;

public record UpdateUserRequest(
    string FullName,
    string Email,
    int Age,
    string PhoneNumber,
    string Location,
    string AdditionalInfo);