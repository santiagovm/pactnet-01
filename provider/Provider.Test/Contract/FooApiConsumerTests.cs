using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using provider;
using provider.Domain;
using provider.FooIntegration;
using Provider.Test.Contract.Plumbing;
using Xunit;

namespace Provider.Test.Contract
{
    public class FooApiConsumerTests : IClassFixture<ConsumerPactClassFixture>
    {
        public FooApiConsumerTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = ConsumerPactClassFixture.MockProviderServiceBaseUri;
        }

        [Fact]
        public async void GetFoo_WhenThereIsAMatch_ReturnsTheFoo()
        {
            // arrange
            const string expectedId = "some-foo-id";
            const string expectedName = "some-foo-name";
            const string expectedDescription = "some-foo-description";

            var expectedFoos = new[]
            {
                new Foo(
                    expectedId,
                    expectedName,
                    expectedDescription
                )
            };

            var fooQuery = new FooQuery(expectedName);

            _mockProviderService
                .Given("There is a foo with some-foo-name")
                .UponReceiving("A GET request to retrieve foo")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/foos",
                    Query = "name=some-foo-name",
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
                    Body = Match.MinType(new
                    {
                        id = Match.Type(expectedId),
                        name = Match.Type(expectedName),
                        description = Match.Type(expectedDescription)
                    }, 1)
                });

            var fooApiClient = new FooApiClient(_mockProviderServiceBaseUri);

            // act
            Foo[] actualFoos = await fooApiClient.GetFoo(fooQuery);

            //assert
            _mockProviderService.VerifyInteractions();
            actualFoos.Should().BeEquivalentTo(expectedFoos.Cast<object>());
        }

        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;
    }
}
