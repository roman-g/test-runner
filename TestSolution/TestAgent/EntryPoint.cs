using Akka.Actor;
using Akka.Configuration;

namespace TestAgent
{
	public class EntryPoint
	{
		private IActorRef testAgent;

		public void Initialize()
		{
			var config = ConfigurationFactory.ParseString(@"
akka {  
    stdout-loglevel = DEBUG
    loglevel = DEBUG
    log-config-on-start = on        
    actor {                
        debug {  
              unhandled = on
        }

        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        dot-netty.tcp {
		    port = 0
		    hostname = localhost
            maximum-frame-size = 4000000b
        }
    }
}
");
			TestAgentSystem.Instance = ActorSystem.Create("TestAgentSystem", config);

			testAgent = TestAgentSystem.Instance.ActorOf<TestAgentActor>("TestAgent");
			
		}
	}
}