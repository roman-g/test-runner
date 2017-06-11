using System.Reflection;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;
using TestCommon;

namespace TestWeb.Services
{
	public class TestServiceProxy
	{
		private ActorSystem proxySystem;

		public TestServiceProxy()
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
		ask-timeout = 10s
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
			proxySystem = ActorSystem.Create("TestServiceProxySystem", config);
		}

		public async Task<string[]> GetAgentNames()
		{
			var agentsListActor = proxySystem.ActorSelection("akka.tcp://TestServiceSystem@localhost:8081/user/TestService/AgentsList");
			var agentNames = await agentsListActor.Ask<AgentListResponse>(new GetAgentList()).ConfigureAwait(false);
			return agentNames.Names;
		}
	}
}