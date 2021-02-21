using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace provider.test.contract
{
    public class ConsumerPactClassFixture : IDisposable
    {
        public IMockProviderService MockProviderService { get; }

        private static int MockServerPort => 9333;
        public static string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerPactClassFixture()
        {
            _pactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });
            _pactBuilder.ServiceConsumer("Another Consumer").HasPactWith("Foo API");
            MockProviderService = _pactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            _pactBuilder.Build();
        }

        private readonly IPactBuilder _pactBuilder;
    }
}
