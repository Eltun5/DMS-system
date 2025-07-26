namespace DepartmentManagementApp.Application.DTOs.Responses;

public record LoginResponse(string AccessToken, int ExpireInMinutes, string RefreshToken);