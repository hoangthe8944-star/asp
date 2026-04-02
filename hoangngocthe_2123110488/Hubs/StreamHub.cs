using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;

namespace hoangngocthe_2123110488.Hubs
{
    public class StreamHub : Hub
    {
        // Khi user vào xem stream
        public async Task JoinStream(int streamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Stream_{streamId}");
            // Logic tăng ViewersCount trong DB hoặc Redis
        }
    }
}
