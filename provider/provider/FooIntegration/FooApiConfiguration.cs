using System;

namespace PactNet01.Provider.FooIntegration
{
    public static class FooApiConfiguration
    {
        public static string BaseUri { get; }

        static FooApiConfiguration()
        {
            string baseUri = Environment.GetEnvironmentVariable("PROVIDER_FOO_API_BASE_URI");

            if (string.IsNullOrWhiteSpace(baseUri))
            {
                throw new Exception("missing environment variable PROVIDER_FOO_API_BASE_URI");
            }

            BaseUri = baseUri;
        }
    }
}
