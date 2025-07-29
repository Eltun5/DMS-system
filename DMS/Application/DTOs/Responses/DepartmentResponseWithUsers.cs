namespace DepartmentManagementApp.Application.DTOs.Responses;

public record DepartmentResponseWithUsers(
        string Id,
        string DepartmentName,
        string Description,
        string ManagerId,
        string ParentId,
        List<UserResponse> Users,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );