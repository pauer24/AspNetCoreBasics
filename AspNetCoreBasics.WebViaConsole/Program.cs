using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace AspNetCoreBasics.WebViaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder()
                .Configure(app =>
                {
                    app.Run(context =>
                    {
                        return context.Response.WriteAsync("Heeeey it's working");
                    });
                }).Build().Run();
        }
    }
}
