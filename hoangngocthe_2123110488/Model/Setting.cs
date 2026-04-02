namespace hoangngocthe_2123110488.Model
{
    public class Setting
    {
        public int Id { get; set; }
        public string Key { get; set; } // "SiteName", "MaxUploadSize"
        public string Value { get; set; }
        public string Group { get; set; } // "General", "Email", "Security"
        public int? UserId { get; set; } // Cấu hình riêng cho user (nếu có)
    }
}
