using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace consumer
{
    public class SomethingApiClient
    {
        public SomethingApiClient(string baseUri = null)
        {
            _baseUri = baseUri ?? "http://my-api";
        }

        public async Task<Something> GetSomething(string id)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["field1"] = "value1"; // santi: receive as parameters
            query["field2"] = "value2";

            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = $"/somethings/{id}",
                Query = query.ToString() ?? string.Empty
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

            return await JsonSerializer.DeserializeAsync<Something>(stream, options);
        }

        private readonly string _baseUri;
    }
}
