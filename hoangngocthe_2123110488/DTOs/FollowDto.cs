namespace hoangngocthe_2123110488.DTOs
{
    public class FollowDto
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public string FollowingUsername { get; set; } = null!;
        public string? FollowingAvatar { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
