using System;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;

namespace Provider.Test.Contract.Plumbing
{
    public class ConsumerPactClassFixture : IDisposable
    {
        public IMockProviderService MockProviderService { get; }

        private static int MockServerPort => 9444; // santi: this cannot match the port in the other test: 9333
        public static string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerPactClassFixture()
        {
            _pactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });
            _pactBuilder.ServiceConsumer("Something API").HasPactWith("Foo API");
            MockProviderService = _pactBuilder.MockService(MockServerPort, host: IPAddress.Any);
        }

        public void Dispose()
        {
            _pactBuilder.Build();
        }

        private readonly IPactBuilder _pactBuilder;
    }
}
