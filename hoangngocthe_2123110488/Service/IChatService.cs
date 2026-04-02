using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository;

namespace hoangngocthe_2123110488.Service
{
    public interface IChatService
    {
        Task<ChatMessageDto> SendMessageAsync(int userId, SendChatRequest request);
        Task<IEnumerable<ChatMessageDto>> GetMessagesAsync(int streamId, int take = 50);
        Task BanUserAsync(int bannedBy, ChatBanRequest request);
        Task<bool> IsUserBannedAsync(int streamId, int userId);
    }

    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepo;
        private readonly IUserRepository _userRepo;

        public ChatService(IChatRepository chatRepo, IUserRepository userRepo)
        {
            _chatRepo = chatRepo;
            _userRepo = userRepo;
        }

        public async Task<ChatMessageDto> SendMessageAsync(int userId, SendChatRequest request)
        {
            if (await _chatRepo.IsUserBannedAsync(request.StreamId, userId))
                throw new Exception("You are banned from this chat.");

            var user = await _userRepo.GetByIdAsync(userId)!;
            var msg = new ChatMessage
            {
                StreamId = request.StreamId,
                UserId = userId,
                Message = request.Message,
                Type = request.Type,
                CreatedAt = DateTime.UtcNow
            };

            await _chatRepo.AddAsync(msg);
            return new ChatMessageDto
            {
                Id = msg.Id,
                StreamId = msg.StreamId,
                UserId = userId,
                Username = user!.Username,
                Avatar = user.Avatar,
                Message = msg.Message,
                Type = msg.Type,
                CreatedAt = msg.CreatedAt
            };
        }

        public async Task<IEnumerable<ChatMessageDto>> GetMessagesAsync(int streamId, int take = 50)
        {
            var messages = await _chatRepo.GetByStreamAsync(streamId, take);
            return messages.Select(m => new ChatMessageDto
            {
                Id = m.Id,
                StreamId = m.StreamId,
                UserId = m.UserId,
                Username = m.User?.Username ?? "",
                Avatar = m.User?.Avatar,
                Message = m.Message,
                Type = m.Type,
                CreatedAt = m.CreatedAt
            });
        }

        public async Task BanUserAsync(int bannedBy, ChatBanRequest request)
        {
            var ban = new ChatBan
            {
                StreamId = request.StreamId,
                UserId = request.UserId,
                BannedBy = bannedBy,
                Reason = request.Reason,
                ExpiredAt = request.ExpiredAt
            };
            await _chatRepo.AddBanAsync(ban);
        }

        public async Task<bool> IsUserBannedAsync(int streamId, int userId)
            => await _chatRepo.IsUserBannedAsync(streamId, userId);
    }
}
