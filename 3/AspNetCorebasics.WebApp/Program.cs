using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreBasics.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost
                .CreateDefaultBuilder()
                .Configure(app =>
                {
                    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

                    app.Use(async (context, next) =>
                    {
                        logger.LogWarning("The start of a request");
                        await next();
                    });

                    app.Map("/something", SecondFlow);

                    app.Use(async (context, next) =>
                    {                        
                        logger.LogWarning("This is a midd step of an appender middleware");
                        await next();
                    });

                    app.Run(async (context) =>
                    {
                        await context.Response.WriteAsync("This is the end");
                    });
                })
                .Build().Run();
        }

        private static void SecondFlow(IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetService<ILogger<Program>>();

            app.Use(async (context, next) =>
            {
                logger.LogWarning("This is a different flow");
                await next();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Wooow! A different flow!!");
            });
        }
    }
}