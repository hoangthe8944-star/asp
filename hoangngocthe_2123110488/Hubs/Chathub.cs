using System.Security.Claims;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Service;

namespace hoangngocthe_2123110488.Hubs

{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService) => _chatService = chatService;

        // Client gọi: connection.invoke("JoinStream", streamId)
        public async Task JoinStream(int streamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"stream-{streamId}");
            await Clients.Caller.SendAsync("Joined", $"Joined stream {streamId}");
        }

        // Client gọi: connection.invoke("LeaveStream", streamId)
        public async Task LeaveStream(int streamId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"stream-{streamId}");
        }

        // Client gọi: connection.invoke("SendMessage", { streamId, message, type })
        public async Task SendMessage(SendChatRequest request)
        {
            var userId = int.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var msg = await _chatService.SendMessageAsync(userId, request);
                // Broadcast đến tất cả người trong stream group
                await Clients.Group($"stream-{request.StreamId}")
                             .SendAsync("ReceiveMessage", msg);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}