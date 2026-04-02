using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data;

namespace hoangngocthe_2123110488.Repository
{
    public interface IFollowRepository : IGenericRepository<Follow>
    {
        Task<bool> IsFollowingAsync(int followerId, int followingId);
        Task<IEnumerable<Follow>> GetFollowersAsync(int userId);
        Task<IEnumerable<Follow>> GetFollowingAsync(int userId);
        Task<int> GetFollowerCountAsync(int userId);
    }

}
