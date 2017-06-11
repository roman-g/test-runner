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
		public static Settings settings;

		static void Main(string[] args)
		{
			var builder = new ContainerBuilder();
			var assembly = Assembly.GetExecutingAssembly();
			builder.RegisterAssemblyTypes(assembly);
			builder.RegisterBuildCallback(x => x.Resolve<EntryPoint>().Initialize());
			builder.Build();
			Console.ReadKey();
		}
	}
}