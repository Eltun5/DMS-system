using WebApplication1.Domain.Enums;

namespace WebApplication1.Domain.Models;

public class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public int Age { get; set; }
    
    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public string? Location { get; set; }

    public double Salary { get; set; }

    public Role Role { get; set; }
    
    public ICollection<Department> Departments { get; set; }
    
    public string? AdditionalInfo { get; set; }
    
    public bool IsActive { get; set; }
    
    public bool IsVerified { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime NextTimeToChangePassword { get; set; }
    
    public DateTime? LastPaidDate { get; set; }
    
    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? DeleteAt { get; set; }
}