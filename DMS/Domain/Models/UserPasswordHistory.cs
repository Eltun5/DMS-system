namespace WebApplication1.Domain.Models;

public class UserPasswordHistory
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Password { get; set; }
    
    public DateTime ChangeTime { get; set; }
}