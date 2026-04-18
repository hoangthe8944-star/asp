using System.Threading.Tasks;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;

namespace hoangngocthe_2123110488.Repository
{
    public interface IStreamRepository : IGenericRepository<Model.Stream>
    {
        Task<IEnumerable<Model.Stream>> GetLiveStreamsAsync();
        Task<IEnumerable<Model.Stream>> GetByStreamerIdAsync(int streamerId);
        Task<Model.Stream?> GetWithDetailsAsync(int streamId);
        Task<IEnumerable<Model.Stream>> SearchAsync(string keyword, int? categoryId);
        Task<Model.Stream?> GetByStreamKeyAsync(string streamKey);
    }
}
