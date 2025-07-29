namespace DepartmentManagementApp.Application.DTOs.Responses;

public record DepartmentResponse (
    string Id,
    string DepartmentName,
    string Description,
    string ManagerId,
    string ParentId,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );