using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Akka.Actor;
using TestCommon;

namespace TestService
{
    public class TestsActor : ReceiveActor, ILogReceive
	{
        private readonly ActorSelection agentListActor;
		//temp trash below...
	    private bool testsHaveDistirbuted = false;
	    private string testDll = null;

        public TestsActor()
        {
            //hide all these strings somehow...
            agentListActor = Context.ActorSelection("../AgentList");
            Receive<RunTestsRequest>(HandleRunTestsRequest);
	        Receive<CheckoutAndBuildCompleted>(HandleCheckoutAndBuildCompleted);
        }

		//trash!
	    private bool HandleCheckoutAndBuildCompleted(CheckoutAndBuildCompleted checkoutAndBuildCompleted)
	    {
		    if (testsHaveDistirbuted)
			    return true;

		    var parseResult = Sender.Ask<ParseTestDllResult>(new ParseTestDll
		    {
			    Dll = testDll
		    }).Result;

		    var agentList = agentListActor.Ask<AgentListResponse>(new GetAgentList()).Result;

		    var distributedTests = parseResult.TestNames
			    .Select((x, i) => new {Index = i, Name = x})
			    .GroupBy(x => x.Index % agentList.AgentActorRefs.Length)
			    .Zip(agentList.AgentActorRefs, (g, actor) =>
				    new
				    {
					    Actor = actor,
					    Tests = g.Select(y => y.Name).ToArray()
				    })
			    .ToArray();

		    foreach (var testGroup in distributedTests)
		    {
			    testGroup.Actor.Tell(new RunTests
			    {
				    TestNames = testGroup.Tests,
					Dll = testDll
			    });
		    }

			testsHaveDistirbuted = true;
		    return true;
	    }

	    private bool HandleRunTestsRequest(RunTestsRequest runTestsRequest)
	    {
		    testDll = runTestsRequest.Dll;
			var agentList = agentListActor.Ask<AgentListResponse>(new GetAgentList()).Result;
            foreach (var agent in agentList.AgentActorRefs)
            {
                agent.Tell(new CheckoutAndBuild
                {
                    Server = runTestsRequest.Server,
                    Branch = runTestsRequest.Branch
                }, Self);
            }
            return true;
        }
    }
	
}