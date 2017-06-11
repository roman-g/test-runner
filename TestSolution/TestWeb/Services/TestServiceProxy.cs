using System;
using System.Reflection;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;
using TestCommon;
using TestWeb.Settings;

namespace TestWeb.Services
{
	public class TestServiceProxy : IDisposable
	{
		private readonly SettingsHolder settingsHolder;
		private readonly ActorSystem proxySystem;

		public TestServiceProxy(SettingsHolder settingsHolder)
		{
			this.settingsHolder = settingsHolder;
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
		ask-timeout = 10s
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
			
			proxySystem = ActorSystem.Create("TestServiceProxySystem", config);
		}

		public async Task<string[]> GetAgentNames()
		{
			var agentsListActor =
				proxySystem.ActorSelection($"akka.tcp://TestServiceSystem@{settingsHolder.AgentSettings.ServiceEndpoint}/user/TestService/AgentsList");
			var agentNames = await agentsListActor.Ask<AgentListResponse>(new GetAgentList()).ConfigureAwait(false);
			return agentNames.Names;
		}

		public void Dispose()
		{
			proxySystem?.Dispose();
		}
	}
}