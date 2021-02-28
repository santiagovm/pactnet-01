using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;
using PactNet01.Provider.Test.Contract.Plumbing;
using Xunit;
using Xunit.Abstractions;

namespace PactNet01.Provider.Test.Contract
{
    public class SomethingApiProviderTests : IDisposable
    {
        private const string TestServiceBaseUri = "https://localhost:9001";
        
        public SomethingApiProviderTests(ITestOutputHelper outputHelper)
        {
            // setup Foo API mock
            _fooApiMockBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });
            _fooApiMockBuilder.ServiceConsumer("Something API2").HasPactWith("Foo API");
            _fooApiMock = _fooApiMockBuilder.MockService(9333, host: IPAddress.Any); // santi: this has to match port in .env, kind of brittle

            // setup Something API to be tested
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

            _fooApiMockBuilder.Build();
        }

        [Fact]
        public void PactflowWebhook_EnsureSomethingApiHonorsContractsWithPublishedPact()
        {
            // arrange
            
            // setup mocked Foo API
            const string guidRegex = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";

            _fooApiMock.ClearInteractions();

            _fooApiMock
                .Given("There is a foo with some-guid")
                .UponReceiving("A GET request to retrieve foo")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/foos",
                    Query = Match.Regex("name=00000000-0000-0000-0000-000000000000", $"^name={guidRegex}$"),
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new[]
                    {
                        new
                        {
                            id = "some id",
                            name = "some name",
                            description = "some description"
                        }
                    }
                });
            
            // get environment variables
            DotNetEnv.Env.TraversePath().Load();
            
            bool isCI = "true".Equals(Environment.GetEnvironmentVariable("CI"));
            string pactBrokerBaseUrl = Environment.GetEnvironmentVariable("PACT_BROKER_BASE_URL");
            string pactBrokerApiToken = Environment.GetEnvironmentVariable("PACT_BROKER_API_TOKEN");
            string pactConsumerName = Environment.GetEnvironmentVariable("PACT_CONSUMER_NAME");
            string pactConsumerTag = Environment.GetEnvironmentVariable("PACT_CONSUMER_TAG");
            string pactProviderTag = Environment.GetEnvironmentVariable("PACT_PROVIDER_TAG");
            string gitCommitShortSha = Environment.GetEnvironmentVariable("GIT_COMMIT_SHORT_SHA");
            
            if (!isCI)
            {
                _outputHelper.WriteLine("test skipped, this test is intended to run only in CI triggered by pactflow webhook");
                return;
            }
            
            // act
            // assert
            var pactVerifier = new PactVerifier(new PactVerifierConfig
            {
                ProviderVersion = gitCommitShortSha,
                PublishVerificationResults = isCI,
                Outputters = new[] { new XUnitOutput(_outputHelper) },
                Verbose = true
            });

            PactUriOptions pactUriOptions = new PactUriOptions().SetBearerAuthentication(pactBrokerApiToken);

            var consumerVersionSelectors = new List<VersionTagSelector>();

            if (!string.IsNullOrWhiteSpace(pactConsumerName))
            {
                consumerVersionSelectors.Add(new VersionTagSelector(pactConsumerTag, pactConsumerName));
            }
            
            pactVerifier
                .ProviderState($"{TestServiceBaseUri}/provider-states")
                .ServiceProvider("Something API", TestServiceBaseUri)
                .PactBroker(
                    pactBrokerBaseUrl,
                    pactUriOptions,
                    true,
                    new []{ "ref/heads/main", "dev", "uat", "prod" },
                    new[] { pactProviderTag },
                    consumerVersionSelectors 
                )
                .Verify();
        }

        private readonly ITestOutputHelper _outputHelper;
        private readonly IWebHost _webHost;

        private readonly IPactBuilder _fooApiMockBuilder;
        private readonly IMockProviderService _fooApiMock;
    }
}
