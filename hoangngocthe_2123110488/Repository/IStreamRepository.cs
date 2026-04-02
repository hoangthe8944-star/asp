namespace hoangngocthe_2123110488.Repository
{
    public interface IStreamRepository
    {
        Task<Stream> GetByStreamKey(string key);
        Task<IEnumerable<Stream>> GetLiveStreams();
        Task AddStream(Stream stream);
    }
}
