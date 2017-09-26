using Identity.API.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * you must must have a method/function name "BuildWebHost" otherwise EF Core Migrations will throw an execption
             * that you need to have an implementation of IDesignTimeDbContextFactory
             */
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseHealthChecks("/hc")
                .Build()
                .InitializeDb();
    }
}
