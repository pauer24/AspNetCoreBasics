using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using AspNetCoreBasics.WebApp;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCorebasics.WebApp
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            CreateDefaultWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateDefaultWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        


        public static void Main2(string[] args)
        {
            WebHost
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddTransient<IMemeService, HardcodedMemeService>();
                }).
                Configure(app =>
                {
                    app.Use(async (context, next) =>
                    {
                        var logger = context.RequestServices.GetService<ILogger<Program>>();
                        logger.LogWarning("This is customized");
                        await next();
                    });

                    app.Run(async (context) =>
                    {
                        var memeService = context.RequestServices.GetService<IMemeService>();
                        await context.Response.WriteAsync(memeService.GiveMeAMemeExclamationMark());
                    });
                })
                .Build().Run();
        } // */
    }
}