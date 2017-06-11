using System;
using System.Reflection;
using Autofac;

namespace TestService
{
	class Program
	{
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