using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqliteStorage;

namespace BeastHunterWebApps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SqliteDbContext context = services.GetRequiredService<SqliteDbContext>();

                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Error on initialize Db: {ex.ToString()}");
                }
            }

                host.Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();

    }
}
