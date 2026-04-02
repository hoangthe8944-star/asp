using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository;

namespace hoangngocthe_2123110488.Service
{
    public interface IDonationService
    {
        Task<DonationDto> DonateAsync(int userId, CreateDonationRequest request);
        Task<IEnumerable<DonationDto>> GetByStreamerAsync(int streamerId);
        Task<decimal> GetTotalAsync(int streamerId);
    }

    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepo;
        private readonly IUserRepository _userRepo;
        private readonly INotificationRepository _notifRepo;

        public DonationService(IDonationRepository donationRepo,
                               IUserRepository userRepo,
                               INotificationRepository notifRepo)
        {
            _donationRepo = donationRepo;
            _userRepo = userRepo;
            _notifRepo = notifRepo;
        }

        public async Task<DonationDto> DonateAsync(int userId, CreateDonationRequest request)
        {
            if (request.Amount <= 0) throw new Exception("Amount must be greater than 0.");

            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new Exception("User not found.");

            var donation = new Donation
            {
                UserId = userId,
                StreamerId = request.StreamerId,
                StreamId = request.StreamId,
                Amount = request.Amount,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow
            };

            await _donationRepo.AddAsync(donation);

            // Notify streamer
            await _notifRepo.AddAsync(new Notification
            {
                UserId = request.StreamerId,
                Type = "donation",
                Title = "New Donation!", // Thêm Title cho Notification nếu Model có
                Message = $"{user.Username} donated {request.Amount:C}.", // SỬA: Content -> Message
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            return new DonationDto
            {
                Id = donation.Id,
                UserId = userId,
                Username = user.Username,
                StreamerId = donation.StreamerId,
                StreamId = donation.StreamId,
                Amount = donation.Amount,
                Message = donation.Message,
                CreatedAt = donation.CreatedAt
            };
        }

        public async Task<IEnumerable<DonationDto>> GetByStreamerAsync(int streamerId)
        {
            // Lấy dữ liệu từ Repo
            var donations = await _donationRepo.GetByStreamerAsync(streamerId);

            // Ép kiểu rõ ràng cho d là hoangngocthe_2123110488.Model.Donation
            return donations.Select((hoangngocthe_2123110488.Model.Donation d) => new DonationDto
            {
                Id = d.Id,
                UserId = d.UserId,
                Username = d.User?.Username ?? "Anonymous",
                StreamerId = d.StreamerId,
                StreamId = d.StreamId,
                Amount = d.Amount,
                Message = d.Message,
                CreatedAt = d.CreatedAt
            });
        }
        public async Task<decimal> GetTotalAsync(int streamerId)
            => await _donationRepo.GetTotalDonationAsync(streamerId);
    }
}