namespace BackgroundService.Common.Entity
{
    public class MailConfiguration
    {
        public string Smtp { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
