namespace Audit.API
{
    public class AuditSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string EventBusConnection { get; set; }
    }
}