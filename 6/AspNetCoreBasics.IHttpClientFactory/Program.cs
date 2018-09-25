using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AspNetCoreBasics.HttpClientFactory
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            var webHost = WebHost.CreateDefaultBuilder()
                .ConfigureServices((builder, services) =>
                {
                    services.Configure<ApiConfiguration>(builder.Configuration);

                    services.AddTransient<NytMostPopularService>();
                    // THE HTTP MESSAGE HANDLERS MUST BE REGISTERED AS WELL
                    services.AddTransient<AddApiKeyQueryParameter>();

                    // REGISTERING CLIENT BY NAME
                    services.AddNytBooksApi();
                    // REGISTERING CLIENT FOR SERVICE
                    services.AddNytMostPopularServiceClient();
                })
                .Configure(app => { })
                .Build();

            // GETTING THE FACTORY
            var factory = webHost.Services.GetService<IHttpClientFactory>();
            // REQUESTING THE CLIENT MANUALLY
            var booksClient = factory.CreateClient("nytBooks");
            var response = await booksClient.GetAsync("lists.json?list=e-book-fiction");
            var result = await response.Content.ReadAsStringAsync();

            // THE CLIENT WILL BE PROVIDED AUTOMAGICALLY
            var service = webHost.Services.GetService<NytMostPopularService>();
            var articleTitles = await service.GetTechArticleTitles(2);
        }        
    }

    public class AddApiKeyQueryParameter : DelegatingHandler
    {
        private readonly ApiConfiguration _apiConfiguration;

        public AddApiKeyQueryParameter(IOptions<ApiConfiguration> apiConfigurationOptions)
        {
            _apiConfiguration = apiConfigurationOptions.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["api-key"] = _apiConfiguration.NytApiKey;
            uriBuilder.Query = query.ToString();

            request.RequestUri = uriBuilder.Uri;

            return await base.SendAsync(request, cancellationToken);
        }
    }

    public static class HttpClientExtensions
    {
        public static void AddNytBooksApi(this IServiceCollection services)
        {
            services.AddHttpClient("nytBooks", c =>
            {
                    c.BaseAddress = new Uri("https://api.nytimes.com/svc/books/v3/");
            }).AddHttpMessageHandler<AddApiKeyQueryParameter>();
        }

        public static void AddNytMostPopularServiceClient(this IServiceCollection services)
        {
            services.AddHttpClient<NytMostPopularService>(c =>
            {
                c.BaseAddress = new Uri("https://api.nytimes.com/svc/mostpopular/v2/mostemailed/Technology/1.json");

            }).AddHttpMessageHandler<AddApiKeyQueryParameter>();
        }
    }
}