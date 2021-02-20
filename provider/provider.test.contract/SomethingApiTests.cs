using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using provider.test.contract.XUnitHelpers;
using Xunit;
using Xunit.Abstractions;

namespace provider.test.contract
{
    public class SomethingApiTests : IDisposable
    {
        private const string TestServiceBaseUri = "https://localhost:9001";
        
        public SomethingApiTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            
            _webHost = WebHost
                      .CreateDefaultBuilder()
                      .UseUrls(TestServiceBaseUri)
                      .UseStartup<TestStartup>()
                      .Build();
            
            _webHost.Start();
        }
        
        public void Dispose()
        {
            _webHost.StopAsync().GetAwaiter().GetResult();
            _webHost.Dispose();
        }
        
        [Fact]
        public void EnsureSomethingApiHonorsPactWithConsumer()
        {
            // arrange
            var config = new PactVerifierConfig
                         {
                             Outputters = new[]
                                          {
                                              new XUnitOutput(_outputHelper)
                                          },
                             Verbose = false
                         };
            
            // act
            // assert
            var pactVerifier = new PactVerifier(config);
            
            // santi: use pact broker
            
            pactVerifier
               .ProviderState($"{TestServiceBaseUri}/provider-states")
               .ServiceProvider("Provider", TestServiceBaseUri)
               .HonoursPactWith("Consumer")
               .PactUri(@"../../../../../consumer/consumer.test.contract/pacts/my_consumer-something_api.json")
               .Verify();
        }

        private readonly ITestOutputHelper _outputHelper;
        private readonly IWebHost _webHost;
    }
}
