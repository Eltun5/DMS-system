namespace WebApplication1.Application.DTOs.Responses;

public record DepartmentResponseWithUsers(
        string Id,
        string DepartmentName,
        string Description,
        string ManagerId,
        List<UserResponse> Users,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );