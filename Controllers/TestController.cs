using System;
using Microsoft.AspNetCore.Mvc;

namespace VStoreAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ContentResult Index()
        {
            return base.Content("<title>Tudo certo por aqui </title>"+
                "<h1>Aparentemente tudo ok! </h1>"+
                "<img src=\"https://i.pinimg.com/originals/b8/c2/55/b8c255488a32607b36acb3af30b27a5f.png\">",
                "text/html");
        }
    }
}
