namespace DepartmentManagementApp.Application.DTOs.Requests;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    int Age,
    string PhoneNumber,
    string Location,
    string AdditionalInfo);