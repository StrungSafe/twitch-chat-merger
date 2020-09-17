﻿namespace Chatr.Console
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
            {
                services.AddLogging();

                services.AddHostedService<Bot>();
                services.Configure<BotConfig>(context.Configuration.GetSection(nameof(BotConfig)));
            });
        }
    }
}