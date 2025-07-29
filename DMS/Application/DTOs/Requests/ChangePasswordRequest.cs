namespace DepartmentManagementApp.Application.DTOs.Requests;

public record ChangePasswordRequest(
    string userId,
    string oldPassword,
    string newPassword);