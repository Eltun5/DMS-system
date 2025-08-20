namespace WebApplication1.Application.DTOs.Responses;

public record DepartmentResponse (
    string Id,
    string DepartmentName,
    string Description,
    string ManagerId,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );