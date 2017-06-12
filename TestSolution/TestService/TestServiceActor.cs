using Akka.Actor;
using TestCommon;

namespace TestService
{
	public class TestServiceActor : ReceiveActor, ILogReceive
	{
		private readonly IActorRef agentListActor;
	    private readonly IActorRef testsActor;

	    public TestServiceActor()
		{
			agentListActor = Context.ActorOf<AgentListActor>("AgentList");
		    testsActor = Context.ActorOf<TestsActor>("Tests");
			Receive<AgentGreeting>(HandleAgentGreeting);
		}

		private bool HandleAgentGreeting(AgentGreeting agentGreeting)
		{
			agentListActor.Tell(new AddAgent
			{
				AgentActor =  agentGreeting.AgentActor
			});
			return true;
		}
	}
}