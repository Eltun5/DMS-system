namespace DepartmentManagementApp.Domain.Models;

public class DepartmentToUser
{
    public Guid Id { get; set; }
    
    public Guid DepartmentId { get; set; }
    
    public Guid UserId { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public DateTime CreatedAt { get; set; }
}