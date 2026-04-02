using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;

namespace hoangngocthe_2123110488.Service
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> SubscribeAsync(int userId, CreateSubscriptionRequest request);
        Task<bool> IsSubscribedAsync(int userId, int streamerId);
        Task<IEnumerable<SubscriptionDto>> GetByUserAsync(int userId);
    }

    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subRepo;
        private readonly IGenericRepository<SubscriptionPlan> _planRepo;

        public SubscriptionService(ISubscriptionRepository subRepo,
                                   IGenericRepository<SubscriptionPlan> planRepo)
        {
            _subRepo = subRepo;
            _planRepo = planRepo;
        }

        public async Task<SubscriptionDto> SubscribeAsync(int userId, CreateSubscriptionRequest request)
        {
            var existing = await _subRepo.GetActiveAsync(userId, request.StreamerId);
            if (existing != null) throw new Exception("Already subscribed.");

            var plan = await _planRepo.GetByIdAsync(request.PlanId)
                ?? throw new Exception("Plan not found.");

            var sub = new Subscription
            {
                UserId = userId,
                StreamerId = request.StreamerId,
                PlanId = request.PlanId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(plan.Duration),
                Status = "active"
            };

            await _subRepo.AddAsync(sub);
            return new SubscriptionDto
            {
                Id = sub.Id,
                UserId = sub.UserId,
                StreamerId = sub.StreamerId,
                StreamerName = "",
                PlanId = sub.PlanId,
                PlanName = plan.Name,
                StartDate = sub.StartDate,
                EndDate = sub.EndDate,
                Status = sub.Status
            };
        }

        public async Task<bool> IsSubscribedAsync(int userId, int streamerId)
            => await _subRepo.GetActiveAsync(userId, streamerId) != null;

        public async Task<IEnumerable<SubscriptionDto>> GetByUserAsync(int userId)
        {
            var subs = await _subRepo.GetByUserAsync(userId);
            return subs.Select(s => new SubscriptionDto
            {
                Id = s.Id,
                UserId = s.UserId,
                StreamerId = s.StreamerId,
                StreamerName = s.Streamer?.Username ?? "",
                PlanId = s.PlanId,
                PlanName = s.Plan?.Name ?? "",
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Status = s.Status
            });
        }
    }
}