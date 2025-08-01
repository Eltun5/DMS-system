namespace WebApplication1.Domain.Models;

public class Message
{
    public Guid Id { get; set; }
    
    public string Content { get; set; }
    
    public Guid SenderId { get; set; }
    
    public Guid ReceiverId { get; set; }
    
    public bool IsRead { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}