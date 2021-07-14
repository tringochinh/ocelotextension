using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("ocelot.json");
                });
        //Host.CreateDefaultBuilder(args)
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<Startup>()
        //        .ConfigureAppConfiguration((hostingContext, config) =>
        //        {
        //            config
        //                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        //                .AddJsonFile("appsettings.json", true, true);
        //            config.AddJsonFile($"ocelot.Development.json", true, true);
        //            //if(hostingContext.HostingEnvironment.EnvironmentName == "Development")
        //            //{
        //            //    config.AddJsonFile($"ocelot.Development.json", true, true);
        //            //} 
        //            //else if (hostingContext.HostingEnvironment.EnvironmentName == "Production")
        //            //{
        //            //    config.AddJsonFile($"ocelot.Production.json", true, true);
        //            //}
        //            //else if (hostingContext.HostingEnvironment.EnvironmentName == "Test")
        //            //{
        //            //    config.AddJsonFile($"ocelot.Test.json", true, true);
        //            //}
        //        });

        //    });

    }
}
