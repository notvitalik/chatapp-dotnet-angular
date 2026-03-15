using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Infrastructure.Realtime;

[Authorize]
public sealed class ChatHub : Hub
{
}
