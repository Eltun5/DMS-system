using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using WebApplication1.Application.Interfaces;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.DBContext;

namespace WebApplication1.SignalR;

public class ChatHub(AppDbContext db,
    IRedisService redisService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        Log.Information("OnConnectedAsync");
        var id = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Log.Information("User id is : " + id);

        if (!string.IsNullOrEmpty(id))
        {
            await redisService.SetContentAsync(id, "ChatHub", Context.ConnectionId, TimeSpan.FromHours(1));
            Log.Information($"User {id} connected with connectionId {Context.ConnectionId}");
        }
        
        var undeliveredMessages = db.Messages
            .Where(m => m.ReceiverId.ToString() == id && !m.IsRead)
            .ToList();
        
        Log.Information("Undelivered messages are : " + undeliveredMessages.Count);

        foreach (var msg in undeliveredMessages)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", msg);
            msg.IsRead = true;
        }
        
        await db.SaveChangesAsync();

        await base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            redisService.DeleteContentAsync(userId, "ChatHub");
            Log.Information($"User {userId} disconnected");
        }
        return base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendMessageToId(string receiverId, string message)
    {
        Log.Information("Sending message to id : " + receiverId);
        var senderClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(senderClaim)) return;

        if (!Guid.TryParse(receiverId, out Guid parsedReceiverId)) return;

        var senderId = new Guid(senderClaim);

        var newMessage = new Message
        {
            SenderId = senderId,
            ReceiverId = parsedReceiverId,
            Content = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        db.Messages.Add(newMessage);
        await db.SaveChangesAsync();

        Log.Information($"Message from {senderId} to {receiverId}: {message}");

        var connectionId = await redisService.GetContentAsync(receiverId, "ChatHub");
        if (connectionId != null)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", newMessage);
            Log.Information("Receiver is online. Message sent via SignalR.");
        }
        else
        {
            Log.Information("Receiver is offline. Message saved to DB only.");
        }
    }

}