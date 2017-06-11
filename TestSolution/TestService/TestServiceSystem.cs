using Akka.Actor;

namespace TestService
{
	public static class TestServiceSystem
	{
		public static ActorSystem Instance { get; set; }
	}
}