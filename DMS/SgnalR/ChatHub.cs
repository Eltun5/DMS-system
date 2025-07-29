using System.Security.Claims;
using DepartmentManagementApp.Domain.Models;
using DepartmentManagementApp.Infrastructure.DBContext;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace WebApplication1;

public class ChatHub : Hub
{
    private readonly AppDbContext _db;
    
    public ChatHub(AppDbContext db)
    {
        _db = db;
    }
    
    public override async Task OnConnectedAsync()
    {
        var id = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Log.Information("User id is : " + id);

        var undeliveredMessages = _db.Messages
            .Where(m => m.ReceiverId.ToString() == id && !m.IsRead)
            .ToList();

        foreach (var msg in undeliveredMessages)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", msg);
            msg.IsRead = true;
        }

        await _db.SaveChangesAsync();

        await base.OnConnectedAsync();
    }


    public async Task SendMessageToId(string receiverId, string message)
    {
        var newMessage = new Message
        {
            SenderId = new Guid(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            ReceiverId = new Guid(receiverId),
            Content = message,
            CreatedAt = DateTime.UtcNow
        };

        _db.Messages.Add(newMessage);
        await _db.SaveChangesAsync();

        await Clients.User(receiverId).SendAsync("ReceiveMessage", newMessage);
        newMessage.IsRead = true;
        await _db.SaveChangesAsync();
    }
}