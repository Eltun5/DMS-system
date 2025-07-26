namespace DepartmentManagementApp.Domain.Models;

public class Department
{
    public Guid Id { get; set; }

    public string DepartmentName { get; set; }

    public string? Description { get; set; }

    public Guid ManagerId { get; set; }

    public Guid ParentId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}