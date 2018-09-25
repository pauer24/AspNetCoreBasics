using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace NetCoreBasics.Configuration
{
    class Program
    {
        private static IConfiguration _hostConfiguration;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()  // path: C:\Users\{current_user}\AppData\Roaming\Microsoft\UserSecrets\{csproj_UserSecretsId}
                .Build();


            // HARD READING CONFIG
            Console.WriteLine($"Configuration RootSetting= {configuration["RootSetting"]}");
            Console.WriteLine($"Configuration rootsetting= {configuration["rootsetting"]}");

            Console.WriteLine($"Configuration WorldSettings section= {configuration["worldsettings"]}");
            Console.WriteLine($"Configuration WorldSettings:Temperature:Min:Location section= {configuration["WorldSettings:Temperature:Min:Location"]}");

            // MAPPING CONFIG TO CLASS
            Console.WriteLine($"Configuration WorldSettings binded section = {configuration.GetSection("WorldSettings").Get<WorldSettings>()}");

            Console.WriteLine($"Configuration Secret = {configuration["OpenSecrets:0"]}");
            
            Console.ReadKey();

            _hostConfiguration = configuration;

            var host = WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(services => services.Configure<WorldSettings>(_hostConfiguration.GetSection("WorldSettings")))
                .Configure(ConfigureApplication)
                .Build();

            // RETRIVING CONFIG FROM IOPTIONS
            var wSettings = host.Services.GetService<IOptions<WorldSettings>>();

            host.Run();

        }

        private static void ConfigureApplication(IApplicationBuilder appBuilder)
        {
            appBuilder.Run(async context =>
            {
                var message = string.Empty;

                // ON EVERY REQUEST IF THE CONFIGURATION CHANGED, THE VALUE WILL CHANGE
                //var options = context.RequestServices.GetService<IOptions<WorldSettings>>().Value;
                var options = context.RequestServices.GetService<IOptionsSnapshot<WorldSettings>>().Value;
                message += $"Options = {options}";

                await context.Response.WriteAsync(message);
            });
        }
    }
}
