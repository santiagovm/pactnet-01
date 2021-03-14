using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PactNet;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace PactNet01.ConsumerApp.Test
{
    public class ProviderApiConsumerTests
    {
        private const int MockServerPort = 9222;
        
        private IPactBuilder _pactBuilder;
        private IMockProviderService _mockProviderService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _pactBuilder = new PactBuilder(new PactConfig {SpecificationVersion = "2.0.0"});
            _pactBuilder.ServiceConsumer("Consumer App").HasPactWith("Provider API");
            _mockProviderService = _pactBuilder.MockService(MockServerPort);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _pactBuilder.Build();
        }

        [Test]
        public async Task GetSomething_TheTesterSomethingExists_ReturnsTheSomething()
        {
            // arrange
            const string expectedId = "83F9262F-28F1-4703-AB1A-8CFD9E8249C9";
            const string expectedFirstName = "some-first-name-03";
            const string expectedLastName = "some-last-name";

            var expectedSomething = new Something(expectedId, expectedFirstName, expectedLastName);

            const string guidRegex = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";

            _mockProviderService
                .Given("Some application state")
                .UponReceiving("A GET request to retrieve something")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = Match.Regex($"/somethings/{expectedId}", $"^\\/somethings\\/{guidRegex}$"),
                    Query = "field1=value1&field2=value2",
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
                    Body = new
                    {
                        id = Match.Type(expectedId),
                        firstName = Match.Type(expectedFirstName),
                        lastName = Match.Type(expectedLastName)
                    }
                });


            var consumer = new SomethingApiClient($"http://localhost:{MockServerPort}");

            // act
            Something actualSomething = await consumer.GetSomething(expectedId).ConfigureAwait(false);

            // assert
            _mockProviderService.VerifyInteractions();
            actualSomething.Should().BeEquivalentTo(expectedSomething);
        }
    }
}
