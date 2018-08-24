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
                    services.AddTransient<AddApiKeyQueryParameter>();

                    services.AddNytBooksApi();
                    services.AddNytMostPopularServiceClient();
                })
                .Configure(app => { })
                .Build();



            var factory = webHost.Services.GetService<IHttpClientFactory>();

            var booksClient = factory.CreateClient("nytBooks");
            var response = await booksClient.GetAsync("lists.json?list=e-book-fiction");
            var result = await response.Content.ReadAsStringAsync();

            var service = webHost.Services.GetService<NytMostPopularService>();
            var articleTitles = await service.GetTechArticleTitles(2);
        }        
    }

    public class NytMostPopularApiClient { }

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