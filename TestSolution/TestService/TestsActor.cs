using System;
using System.Threading.Tasks;
using Akka.Actor;
using TestCommon;

namespace TestService
{
    public class TestsActor : ReceiveActor
    {
        private readonly ActorSelection agentListActor;

        public TestsActor()
        {
            //hide all these strings somehow...
            agentListActor = Context.ActorSelection("../AgentList");
            ReceiveAsync<RunTestsRequest>(HandleRunTestsRequest);
        }

        private async Task<bool> HandleRunTestsRequest(RunTestsRequest runTestsRequest)
        {
            var agentList = await agentListActor.Ask<AgentListResponse>(new GetAgentList()).ConfigureAwait(false);
            foreach (var agent in agentList.AgentActorRefs)
            {
                agent.Tell(new CheckoutAndBuild
                {
                    Server = runTestsRequest.Server,
                    Branch = runTestsRequest.Branch
                });
            }
            return true;
        }
    }
}