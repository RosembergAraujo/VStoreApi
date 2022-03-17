using System;
using Microsoft.AspNetCore.Mvc;

namespace VStoreAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class TestController : ControllerBase
    {
        [HttpGet] public IActionResult Hello() => Ok(new {Hello = Environment.GetEnvironmentVariable("PORT")});
    }
}