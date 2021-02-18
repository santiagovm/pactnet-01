using System;
using Microsoft.AspNetCore.Mvc;

namespace provider.Controllers
{
    [Route("somethings")]
    public class SomethingController : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(string id)
        {
            return new OkObjectResult(new
                                      {
                                          id = "some-id",
                                          firstName = "some-first-name",
                                          lastName = "some-last-name"
                                      });
        }
    }
}
