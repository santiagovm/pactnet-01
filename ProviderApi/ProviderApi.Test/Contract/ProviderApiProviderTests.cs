using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;
using PactNet01.ProviderApi.Test.Contract.Plumbing;
using Xunit;
using Xunit.Abstractions;

namespace PactNet01.ProviderApi.Test.Contract
{
    public class ProviderApiProviderTests : IDisposable
    {
        private const string TestServiceBaseUri = "https://localhost:9001";
        
        public ProviderApiProviderTests(ITestOutputHelper outputHelper)
        {
            // setup Foo API mock
            _fooApiMockBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });
            _fooApiMockBuilder.ServiceConsumer("Provider API").HasPactWith("Foo API");
            _fooApiMock = _fooApiMockBuilder.MockService(9333, host: IPAddress.Any);

            // setup Provider API to be tested
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
        public void PactflowWebhook_EnsureProviderApiHonorsContractsWithPublishedPact()
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
            
            bool isRunningInPipeline = "true".Equals(Environment.GetEnvironmentVariable("CI"));
            string pactBrokerBaseUrl = Environment.GetEnvironmentVariable("PACT_BROKER_BASE_URL");
            string pactBrokerApiToken = Environment.GetEnvironmentVariable("PACT_BROKER_API_TOKEN");
            string pactProviderTag = Environment.GetEnvironmentVariable("PACT_PROVIDER_TAG");
            string pactUrl = Environment.GetEnvironmentVariable("PACT_URL");
            string gitCommitShortSha = Environment.GetEnvironmentVariable("GIT_COMMIT_SHORT_SHA");
            
            var pactVerifier = new PactVerifier(new PactVerifierConfig
            {
                ProviderVersion = gitCommitShortSha,
                PublishVerificationResults = isRunningInPipeline,
                Outputters = new[] { new XUnitOutput(_outputHelper) },
                Verbose = true
            });

            PactUriOptions pactUriOptions = new PactUriOptions().SetBearerAuthentication(pactBrokerApiToken);

            pactVerifier
               .ProviderState($"{TestServiceBaseUri}/provider-states")
               .ServiceProvider("Provider API", TestServiceBaseUri);

            // pactUrl is provided to pipeline triggered by pactflow webhook indicating what specific pact should be verified
            // if value is missing then default verification is performed for all consumers with tags: main, dev, uat, prod

            bool isVerifyingSpecificPact = !string.IsNullOrWhiteSpace(pactUrl);
            
            if (isVerifyingSpecificPact)
            {
                pactVerifier.PactUri(pactUrl, pactUriOptions);
            }
            else
            {
                var consumerVersionSelectors = new List<VersionTagSelector>
                                               {
                                                   new VersionTagSelector("refs/heads/main", latest: true),
                                                   new VersionTagSelector("dev", latest: true),
                                                   new VersionTagSelector("uat", latest: true),
                                                   new VersionTagSelector("prod", latest: true)
                                               };

                pactVerifier.PactBroker(pactBrokerBaseUrl,
                                        pactUriOptions,
                                        true,
                                        providerVersionTags: new[] { pactProviderTag },
                                        consumerVersionSelectors: consumerVersionSelectors);
            }
            
            // act
            // assert
            pactVerifier.Verify();
        }

        private readonly ITestOutputHelper _outputHelper;
        private readonly IWebHost _webHost;

        private readonly IPactBuilder _fooApiMockBuilder;
        private readonly IMockProviderService _fooApiMock;
    }
}
