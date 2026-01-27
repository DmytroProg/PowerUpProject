using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PowerUp.Api;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendPrivateMessage(string user, string message)
    {
        var sender = Context.UserIdentifier;
        
        await Clients.User(user).SendAsync("ReceivePrivateMessage", sender, message);
    }
}