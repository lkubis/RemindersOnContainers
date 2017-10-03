namespace WebMVC
{
    public class AppSettings
    {
        public Connectionstrings ConnectionStrings { get; set; }
        public string IdentityUrl { get; set; }
        public string ReminderUrl { get; set; }
        public string UditUrl { get; set; }
    }

    public class Connectionstrings
    {
        public string DefaultConnection { get; set; }
    }

    public class HealthCheck
    {
        public int Timeout { get; set; }
    }
}