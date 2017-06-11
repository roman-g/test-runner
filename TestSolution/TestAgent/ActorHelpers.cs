using Akka.Actor;

namespace TestAgent
{
	internal static class ActorHelpers
	{
		public static ActorSelection SelectTestServiceActor(this IUntypedActorContext context)
		{
			return context.ActorSelection("akka.tcp://TestServiceSystem@localhost:8081/user/TestService");
		}
	}
}