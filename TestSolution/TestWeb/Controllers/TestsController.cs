using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWeb.Services;

namespace TestWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Tests/[action]")]
    public class TestsController : Controller
    {
        private readonly TestServiceProxy testServiceProxy;

        public TestsController(TestServiceProxy testServiceProxy)
        {
            this.testServiceProxy = testServiceProxy;
        }

        [HttpPost]
        public void Run([FromBody] RunParams @params)
        {
            testServiceProxy.RunTests(@params.Branch, @params.Server);
        }

        public class RunParams
        {
            public string Branch { get; set; }
            public string Server { get; set; }
        }
    }
}