using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AspNetCoreBasics.HttpClientFactory
{
    public class NytMostPopularService
    {
        private readonly HttpClient _httpClient;

        public NytMostPopularService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<string[]> GetTechArticleTitles(int timePeriod)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress);
            var query = HttpUtility.ParseQueryString(builder.Query);

            query["section"] = "Technology";
            query["time-period"] = timePeriod.ToString();

            builder.Query = query.ToString();

            var response = await _httpClient.GetAsync(builder.Uri);

            var result = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(result);
            var articles = jObject["results"].Children().Select(c => c["title"].Value<string>());

            return articles.ToArray();
        }
    }
}
