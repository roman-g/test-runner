using Akka.Actor;
using Akka.Configuration;

namespace TestService
{
	public class EntryPoint
	{
		private IActorRef testServiceActor;

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

              receive = on 
              autoreceive = on
              lifecycle = on
              event-stream = on
              unhandled = on
        }

        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        dot-netty.tcp {
            port = 8081
            hostname = 0.0.0.0
            public-hostname = localhost
            maximum-frame-size = 4000000b
        }
    }
}
");

			TestServiceSystem.Instance = ActorSystem.Create("TestServiceSystem", config);
			this.testServiceActor = TestServiceSystem.Instance.ActorOf<TestServiceActor>("TestService");
			
		}
	}
}