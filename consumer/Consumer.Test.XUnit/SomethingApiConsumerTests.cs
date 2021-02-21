using System.Collections.Generic;
using FluentAssertions;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace PactNet01.Consumer.Test.XUnit
{
    public class SomethingApiConsumerTests : IClassFixture<ConsumerPactClassFixture>
    {
        public SomethingApiConsumerTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = ConsumerPactClassFixture.MockProviderServiceBaseUri;
        }
        
        [Fact]
        public async void GetSomething_TheTesterSomethingExists_ReturnsTheSomething()
        {
            // arrange
            const string expectedId = "83F9262F-28F1-4703-AB1A-8CFD9E8249C9";
            const string expectedFirstName = "some-first-name";
            const string expectedLastName = "some-last-name";

            var expectedSomething = new Something(
                expectedId,
                expectedFirstName,
                expectedLastName
            );
            
            const string guidRegex = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";

            _mockProviderService
               .Given("There is a something with id 'tester")
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
            
            var somethingApiClient = new SomethingApiClient(_mockProviderServiceBaseUri);
            
            // act
            Something actualSomething = await somethingApiClient.GetSomething(expectedId);

            // assert
            _mockProviderService.VerifyInteractions();
            actualSomething.Should().BeEquivalentTo(expectedSomething);
        }

        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;
    }
}
