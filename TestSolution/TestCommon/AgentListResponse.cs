using Akka.Actor;

namespace TestCommon
{
	public class AgentListResponse
	{
		public IActorRef[] AgentActorRefs { get; set; }
	}
}