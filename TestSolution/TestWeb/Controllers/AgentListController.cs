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
    [Route("api/AgentList")]
    public class AgentListController : Controller
    {
	    private readonly TestServiceProxy testServiceProxy;

	    public AgentListController(TestServiceProxy testServiceProxy)
	    {
		    this.testServiceProxy = testServiceProxy;
	    }

	    public async Task<JsonResult> GetList()
	    {
		    var names = await testServiceProxy.GetAgentNames().ConfigureAwait(false);
			return new JsonResult(new
			{
				names
			});
	    }
    }
}