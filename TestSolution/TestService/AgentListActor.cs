using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using TestCommon;

namespace TestService
{
	public class AgentListActor : ReceiveActor
	{
		private readonly List<Agent> agents = new List<Agent>();

		public AgentListActor()
		{
			Receive<AddAgent>(agent =>
			{
				agents.Add(new Agent
				{
					AgentActor = agent.AgentActor
				});
				Console.Out.WriteLine("agent added, ref:" + agent.AgentActor.ToString());
			});

			Receive<GetAgentList>(request =>
			{
				Console.Out.WriteLine("GetAgentList received");

				Sender.Tell(new AgentListResponse
				{
					Names = agents.Select(x => x.AgentActor.ToString()).ToArray()
				});
			});

			Console.Out.WriteLine("AgentListActor started");
		}

		private class Agent
		{
			public IActorRef AgentActor { get; set; }
		}
	}
}