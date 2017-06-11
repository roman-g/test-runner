using Akka.Actor;

namespace TestAgent
{
	internal static class ActorHelpers
	{
		public static ActorSelection SelectTestServiceActor(this IUntypedActorContext context, string serviceEndpoint)
		{
			return context.ActorSelection($"akka.tcp://TestServiceSystem@{serviceEndpoint}/user/TestService");
		}
	}
}