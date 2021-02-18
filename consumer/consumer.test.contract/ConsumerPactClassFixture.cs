using System;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;

namespace consumer.test.contract
{
    public class ConsumerPactClassFixture : IDisposable
    {
        public IMockProviderService MockProviderService { get; }

        private static int MockServerPort => 9222;
        public static string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerPactClassFixture()
        {
            _pactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });
            _pactBuilder.ServiceConsumer("My Consumer").HasPactWith("Something API");
            MockProviderService = _pactBuilder.MockService(MockServerPort, host: IPAddress.Any);
        }

        public void Dispose()
        {
            _pactBuilder.Build();
        }
        
        private readonly IPactBuilder _pactBuilder;
    }
}
