namespace AuthServices.Domain.Configurations
{
    public class MailSetting
    {
        public string Server { get; set; }

        public int Port { get; set; } = 25;

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public bool Security { get; set; }
    }
}
