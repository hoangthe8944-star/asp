namespace hoangngocthe_2123110488.DTOs
{
    public class MediaMTXRequest
    {
        public string Action { get; set; }   // "publish", "user_disconnect", ...
        public string Path { get; set; }     // "live/afb374..."
        public string Protocol { get; set; } // "rtmp", "hls", ...
        public string Ip { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
