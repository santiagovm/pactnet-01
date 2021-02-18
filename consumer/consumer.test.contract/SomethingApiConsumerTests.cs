using System;
using System.Collections.Generic;
using FluentAssertions;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

// santi: research multiple OS environments
// https://github.com/pact-foundation/pact-workshop-dotnet-core-v1#nb---multiple-os-environments

namespace consumer.test.contract
{
    public class SomethingApiConsumerTests : IClassFixture<ConsumerPactClassFixture>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;
        
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
            
            var expectedSomething = new Something
                                    {
                                        id = expectedId,
                                        firstName = expectedFirstName,
                                        lastName = expectedLastName
                                    };
            
            const string guidRegex = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";

            _mockProviderService
               .Given("There is a something with id 'tester")
               .UponReceiving("A GET request to retrieve something")
               .With(new ProviderServiceRequest
                     {
                         Method = HttpVerb.Get,
                         Path = Match.Regex($"/somethings/{expectedId}", $"^\\/somethings\\/{guidRegex}$"),
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
            
            var consumer = new SomethingApiClient(_mockProviderServiceBaseUri);
            
            // act
            Something result = await consumer.GetSomething(expectedId);

            // assert
            _mockProviderService.VerifyInteractions();
            expectedSomething.Should().BeEquivalentTo(result);
        }
    }
}
