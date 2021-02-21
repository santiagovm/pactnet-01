using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using provider.test.contract.XUnitHelpers;
using Xunit;
using Xunit.Abstractions;

namespace provider.test.contract
{
    public class SomethingApiProviderContractTests : IDisposable
    {
        private const string TestServiceBaseUri = "https://localhost:9001";
        
        public SomethingApiProviderContractTests(ITestOutputHelper outputHelper)
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
        public void EnsureSomethingApiHonorsPactsWithConsumers()
        {
            // arrange
            var config = new PactVerifierConfig
                         {
                             Outputters = new[]
                                          {
                                              new XUnitOutput(_outputHelper)
                                          },
                             ProviderVersion = "0.1.0", // santi: git version?
                             PublishVerificationResults = true,
                             Verbose = false
                         };
            
            // act
            // assert
            var pactVerifierConsumerXUnit = new PactVerifier(config);
            var pactVerifierConsumerNUnit = new PactVerifier(config);
            
            // santi: use pact broker
            
            pactVerifierConsumerXUnit
               .ProviderState($"{TestServiceBaseUri}/provider-states")
               .ServiceProvider("Something API", TestServiceBaseUri)
               .HonoursPactWith("My Consumer")
               .PactUri(@"../../../../../consumer/consumer.test.contract/pacts/my_consumer-something_api.json")
               .Verify();

            pactVerifierConsumerNUnit
                .ProviderState($"{TestServiceBaseUri}/provider-states")
                .ServiceProvider("Something API", TestServiceBaseUri)
                .HonoursPactWith("My Consumer NUnit")
                .PactUri(@"../../../../../consumer/consumer.test.contract.nunit/pacts/my_consumer_nunit-something_api.json")
                .Verify();
        }

        private readonly ITestOutputHelper _outputHelper;
        private readonly IWebHost _webHost;
    }
}
