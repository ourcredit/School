using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace School.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var conf = new ConfigurationBuilder()
                .AddJsonFile("server.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
      
    }
}
