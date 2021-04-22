using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.IO;

namespace GTGrimServer
{
    public class Program
    {
        public static readonly RecyclableMemoryStreamManager StreamManager = new RecyclableMemoryStreamManager();

        public static void Main(string[] args)
        {
            Console.WriteLine("-- GTGrimServer - Open-source Gran Turismo 5 and 6 Server --");
            Console.WriteLine("Init: Starting host");

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
