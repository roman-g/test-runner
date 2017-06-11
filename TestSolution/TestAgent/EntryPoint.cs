using System;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;

namespace TestAgent
{
	public class EntryPoint
	{
		private IActorRef testAgent;

		public void Initialize()
		{
			var config = ConfigurationFactory.ParseString($@"
akka {{  
    stdout-loglevel = DEBUG
    loglevel = DEBUG
    log-config-on-start = on        
    actor {{                
        debug {{  
              unhandled = on
        }}

        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }}
    remote {{
        dot-netty.tcp {{
		    port = 0
		    hostname = {Environment.MachineName}
            maximum-frame-size = 4000000b
        }}
    }}
}}
");
			TestAgentSystem.Instance = ActorSystem.Create("TestAgentSystem", config);
			//o_O
			new AutoFacDependencyResolver(ContainerHolder.Instance, TestAgentSystem.Instance);

			var testAgentProps = TestAgentSystem.Instance.DI().Props<TestAgentActor>();
			testAgent = TestAgentSystem.Instance.ActorOf(testAgentProps, "TestAgent");
		}
	}
}