using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace PactNet01.ConsumerApp
{
    public class SomethingApiClient
    {
        public SomethingApiClient(string baseUri = null)
        {
            _baseUri = baseUri ?? "http://my-api";
        }

        public async Task<Something> GetSomething(string id)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["field1"] = "value1";
            queryString["field2"] = "value2";

            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = $"/somethings/{id}",
                Query = queryString.ToString() ?? string.Empty
            };

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using HttpResponseMessage response = await httpClient.GetAsync(uriBuilder.Uri);

            HttpStatusCode status = response.StatusCode;

            if (status != HttpStatusCode.OK)
            {
                throw new Exception(response.ReasonPhrase);
            }

            await using Stream stream = await response.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return await JsonSerializer.DeserializeAsync<Something>(stream, options);
        }

        private readonly string _baseUri;
    }
}
