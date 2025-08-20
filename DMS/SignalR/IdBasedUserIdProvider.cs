using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.SignalR;

public class IdBasedUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}