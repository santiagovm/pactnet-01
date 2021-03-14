using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PactNet01.ProviderApi.Test.Contract.Plumbing
{
    public class ProviderStateMiddleware
    {
        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;

            _providerStates = new Dictionary<string, Action>
                              {
                                  { "There is no data", RemoveAllData },
                                  { "There is data", AddData }
                              };
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                await _next(context);
            }
        }

        private async void HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int) HttpStatusCode.OK;

            if (!string.Equals(context.Request.Method, HttpMethods.Post, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            if (context.Request.Body == null)
            {
                return;
            }

            var providerState = await JsonSerializer.DeserializeAsync<ProviderState>(context.Request.Body);

            if (providerState == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(providerState.State))
            {
                return;
            }

            if (providerState.Consumer != "Consumer")
            {
                return;
            }
            
            _providerStates[providerState.State].Invoke();
        }
        
        private static void RemoveAllData()
        {
            Console.WriteLine(" >>>>> state change to remove all data");
        }
        
        private static void AddData()
        {
            Console.WriteLine(" >>>>> state change to add data");
        }
        
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;
    }
}
