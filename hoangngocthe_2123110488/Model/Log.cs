namespace hoangngocthe_2123110488.Model
{
    public class Log
    {
        public int Id { get; set; }
        public string Level { get; set; } // "Info", "Warning", "Error"
        public string Message { get; set; }
        public string Exception { get; set; }
        public string ActionBy { get; set; } // Username hoặc IP
        public DateTime CreatedAt { get; set; }
    }
}
