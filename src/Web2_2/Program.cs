using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web2_2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                //根据环境变量加载不同的JSON配置
                config.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                        true, true);
                //从环境变量添加配置
                config.AddEnvironmentVariables();
                config.AddXmlFile("sqlMapper.xml", true, false);
            })
            .UseStartup<Startup>();
    }
}
