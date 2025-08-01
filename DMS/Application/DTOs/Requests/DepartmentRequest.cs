namespace WebApplication1.Application.DTOs.Requests;

public record DepartmentRequest(
    string Name,
    string Description,
    string ManagerId
    );