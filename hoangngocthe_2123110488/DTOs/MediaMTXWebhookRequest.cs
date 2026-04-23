namespace hoangngocthe_2123110488.DTOs
{
    public class MediaMTXWebhookRequest
    {
        public string Action { get; set; } // "publish" hoặc "user_disconnect"
        public string Path { get; set; }   // "live/afb374..."
        public string User { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
    }
}
