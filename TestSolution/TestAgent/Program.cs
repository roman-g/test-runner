using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Autofac;

namespace TestAgent
{
	class Program
	{
		public static AgentSettings agentSettings;

		static void Main(string[] args)
		{
			var builder = new ContainerBuilder();
			var assembly = Assembly.GetExecutingAssembly();
			builder.RegisterAssemblyTypes(assembly);
			ContainerHolder.Instance = builder.Build();
			ContainerHolder.Instance.Resolve<EntryPoint>().Initialize();
			Console.ReadKey();
		}
	}
}