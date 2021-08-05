using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace PointerApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseCloudHosting()
            .AddCloudFoundryConfiguration()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
