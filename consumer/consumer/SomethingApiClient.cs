using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace consumer
{
    public class SomethingApiClient
    {
        public SomethingApiClient(string baseUri = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://my-api") };
        }

        public async Task<Something> GetSomething(string id)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/somethings/{id}");
            request.Headers.Add("Accept", "application/json");

            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            
            string content = await response.Content.ReadAsStringAsync();
            HttpStatusCode status = response.StatusCode;
            string reasonPhrase = response.ReasonPhrase;

            if (status != HttpStatusCode.OK)
            {
                throw new Exception(reasonPhrase);
            }

            return !string.IsNullOrEmpty(content) ? JsonSerializer.Deserialize<Something>(content) : null;
        }
        
        private readonly HttpClient _httpClient;
    }
}
