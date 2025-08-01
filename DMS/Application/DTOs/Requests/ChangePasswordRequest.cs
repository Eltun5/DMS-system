namespace WebApplication1.Application.DTOs.Requests;

public record ChangePasswordRequest(
    string userId,
    string oldPassword,
    string newPassword);