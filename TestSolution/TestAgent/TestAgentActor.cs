using System;
using Akka.Actor;
using TestCommon;

namespace TestAgent
{
	public class TestAgentActor : ReceiveActor, ILogReceive
	{
		private readonly ActorSelection server;

		public TestAgentActor(SettingsHolder settingsHolder)
		{
			Console.WriteLine("AgentActor Started");
			server = Context.SelectTestServiceActor(settingsHolder.AgentSettings.ServiceEndpoint);
			server.Tell(new AgentGreeting
			{
				AgentActor = Self
			});
		}
		

		/*
		private readonly Regex resultRegex = new Regex("<<TEST_FINISHED>>:(?'testName'[^:]*):(?'testResult'[^:]*)");
		

		public void Handle(RunTests message)
		{
			var testNames = string.Join(",", message.TestNames);
			Console.WriteLine($"Starting process for {testNames}");
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = Program.settings.NunitConsoleExePath,
					Arguments = $"{Program.settings.TestDllPath} /run={testNames}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				}
			};
			process.Start();

			Console.Out.WriteLine("Process started");

			//to another actor
			while (!process.StandardOutput.EndOfStream)
			{
				var readLine = process.StandardOutput.ReadLine();
				var match = resultRegex.Match(readLine);
				if (match.Success)
					Sender.Tell(new TestFinished
					{
						TestName = match.Groups["testName"].Value,
						Result = match.Groups["testResult"].Value
					});
				Console.Out.WriteLine(readLine);
			}

			new Thread(() =>
			{
				while (!process.StandardError.EndOfStream)
					Console.Out.WriteLine(process.StandardError.ReadLine());
			}).Start();

			process.WaitForExit();
			Console.WriteLine("Process finished " + process.ExitCode);
		}

		public void Handle(Parse message)
		{
			Console.WriteLine("Handling Parse message");
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;

			var testAassembly = Assembly.ReflectionOnlyLoadFrom(Program.settings.TestDllPath);
			var methods = testAassembly.DefinedTypes
				.SelectMany(t => t.GetMethods())
				.Where(m => m.GetCustomAttributesData()
					.Any(x => x.AttributeType.FullName == "NUnit.Framework.TestAttribute"))
				.Select(x => $"{x.ReflectedType.FullName}.{x.Name}")
				.ToArray();

			Sender.Tell(new ParseResult {TestNames = methods}, Self);
		}

		private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
		{
			var directory = Path.GetDirectoryName(Program.settings.TestDllPath);
			var dllpath = Path.Combine(directory, args.Name.Split(',').First() + ".dll");
			try
			{
				return Assembly.ReflectionOnlyLoadFrom(dllpath);
			}
			catch (FileNotFoundException)
			{
				return Assembly.ReflectionOnlyLoad(args.Name);
			}
		}*/
	}
}