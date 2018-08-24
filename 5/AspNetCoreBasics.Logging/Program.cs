using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreBasics.Logging.Services;
using System;
using NLog.Web;



namespace NetCoreBasics.Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddTransient<ServiceThatLogs>();
                }).ConfigureLogging(logBuilder =>
                {

                    // Si ningún otro filtro dice lo contrario, no se logará nada de esa clase
                    //logBuilder.AddFilter("NetCoreBasics.Logging.Logging", LogLevel.None);

                    // No se logará nada en consola que venga de Microsoft.* como son las peticiones HTTP 
                    //logBuilder.AddFilter<Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>("Microsoft.", LogLevel.None);

                    // Se logará todo en la consola de Debug, independientemente de la categoría de log
                    //logBuilder.AddFilter<Microsoft.Extensions.Logging.Debug.DebugLoggerProvider>(logLevel => true);


                    // Do not log anything in the Console: Equivalent ways
                    //logBuilder.AddFilter<Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>(logLevel => false);
                    //logBuilder.AddFilter((provider, category, logLevel) =>
                    //{
                    //    if (provider.StartsWith("Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider"))
                    //    {
                    //        return false;
                    //    }
                    //    return true;
                    //});
                })
                .UseNLog()
                .Configure(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var serviceThatLogs = context.RequestServices.GetService<ServiceThatLogs>();
                        serviceThatLogs.LogSomethingInCritical();
                        serviceThatLogs.LogSomethingInDebug();
                        serviceThatLogs.LogSomethingInError();
                        serviceThatLogs.LogSomethingInInfo();
                        serviceThatLogs.LogSomethingInWarning();
                        serviceThatLogs.LogAnException();


                        await context.Response.WriteAsync("Hello with a basic response");
                    });
                }).Build().Run();
        }
    }
}
