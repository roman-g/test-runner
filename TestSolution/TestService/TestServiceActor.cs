using Akka.Actor;
using TestCommon;

namespace TestService
{
	public class TestServiceActor : ReceiveActor
	{
		private readonly IActorRef agentsListActor;

		public TestServiceActor()
		{
			agentsListActor = Context.ActorOf<AgentListActor>("AgentsList");
			Receive<AgentGreeting>(HandleAgentGreeting);
		}

		private bool HandleAgentGreeting(AgentGreeting agentGreeting)
		{
			agentsListActor.Tell(new AddAgent
			{
				AgentActor =  agentGreeting.AgentActor
			});
			return true;
		}
	}
}