using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace provider
{
    public class FooApiClient
    {
        public FooApiClient(string baseUri)
        {
            _baseUri = baseUri ?? "http://my-api";
        }

        public async Task<Foo[]> GetFoo(FooQuery fooQuery)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["name"] = fooQuery.Name;

            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = "/foos",
                Query = queryString.ToString() ?? string.Empty
            };

            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uriBuilder.Uri
            };

            request.Headers.Add("Accept", "application/json");

            using var httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.SendAsync(request);

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

            return await JsonSerializer.DeserializeAsync<Foo[]>(stream, options);
        }

        private readonly string _baseUri;
    }
}
