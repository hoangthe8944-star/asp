namespace hoangngocthe_2123110488.DTOs
{
    public class SettingDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }

    public class UpsertSettingRequest
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
