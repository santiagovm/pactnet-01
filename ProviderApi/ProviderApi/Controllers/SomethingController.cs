using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PactNet01.ProviderApi.Domain;
using PactNet01.ProviderApi.FooIntegration;

namespace PactNet01.ProviderApi.Controllers
{
    [Route("somethings")]
    public class SomethingController : Controller
    {
        public SomethingController(FooApiClient fooApiClient)
        {
            _fooApiClient = fooApiClient;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Foo[] foos = await _fooApiClient.GetFoo(new FooQuery(id));
            Foo theFoo = foos[0];

            var something = new Something(theFoo.Id, theFoo.Name, theFoo.Description);

            return new OkObjectResult(something);
        }

        private readonly FooApiClient _fooApiClient;
    }
}
