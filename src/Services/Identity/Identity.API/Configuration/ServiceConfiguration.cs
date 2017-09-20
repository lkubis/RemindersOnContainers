using Identity.API.Configuration.Sections;

namespace Identity.API.Configuration
{
    public class ServiceConfiguration
    {
        public ConnectionStringOptions ConnectionStrings { get; set; }
        public IdentityOptions IdentityOptions { get; set; }
    }
}