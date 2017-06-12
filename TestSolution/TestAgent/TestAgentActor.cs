using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Akka.Actor;
using TestCommon;

namespace TestAgent
{
	public class TestAgentActor : ReceiveActor, ILogReceive
	{
		private readonly SettingsHolder settingsHolder;
		private readonly HgService hgService;
		private readonly BuildService buildService;
		private readonly ActorSelection server;

		public TestAgentActor(SettingsHolder settingsHolder, HgService hgService, BuildService buildService)
		{
			this.settingsHolder = settingsHolder;
			this.hgService = hgService;
			this.buildService = buildService;
			Console.WriteLine("AgentActor Started");
			server = Context.SelectTestServiceActor(settingsHolder.AgentSettings.ServiceEndpoint);
			server.Tell(new AgentGreeting
			{
				AgentActor = Self
			});

			Receive<CheckoutAndBuild>(HandleCheckoutAndBuild);

			Receive<ParseTestDll>(HandleParseTestDll);

			Receive<RunTests>(HandleRunTests);
		}

		private bool HandleParseTestDll(ParseTestDll parseTestDll)
		{
			Console.WriteLine("Handling Parse message");

			var testDll = Path.Combine(AgentHelpers.GetWorkingDirectory(), parseTestDll.Dll);
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainReflectionOnlyAssemblyResolve;
			var testAassembly = Assembly.ReflectionOnlyLoadFrom(testDll);
			var methods = testAassembly.DefinedTypes
				.SelectMany(t => t.GetMethods())
				.Where(m => m.GetCustomAttributesData()
					.Any(x => x.AttributeType.FullName == "NUnit.Framework.TestAttribute"))
				.Select(x => $"{x.ReflectedType.FullName}.{x.Name}")
				.ToArray();

			Sender.Tell(new ParseTestDllResult {TestNames = methods}, Self);


			Assembly CurrentDomainReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
			{
				var directory = Path.GetDirectoryName(testDll);
				var dllpath = Path.Combine(directory, args.Name.Split(',').First() + ".dll");
				try
				{
					return Assembly.ReflectionOnlyLoadFrom(dllpath);
				}
				catch (FileNotFoundException)
				{
					return Assembly.ReflectionOnlyLoad(args.Name);
				}
			}

			return true;
		}

		private bool HandleCheckoutAndBuild(CheckoutAndBuild checkoutAndBuild)
		{
			hgService.CloneOrUpdate(checkoutAndBuild.Server, checkoutAndBuild.Branch);
			buildService.Build();
			Sender.Tell(new CheckoutAndBuildCompleted());
			return true;
		}

		private bool HandleRunTests(RunTests runTests)
		{
			var testDll = Path.Combine(AgentHelpers.GetWorkingDirectory(), runTests.Dll);
			var testNames = string.Join(",", runTests.TestNames);
			Console.WriteLine($"Starting process for {testNames}");
			Console.WriteLine($"{settingsHolder.AgentSettings.NunitConsoleExePath} {testDll} /run={testNames}");
			//extract this blocking call from the message handler
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = settingsHolder.AgentSettings.NunitConsoleExePath,
					Arguments = $"{testDll} /run={testNames}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				}
			};
			process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
			process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			process.WaitForExit();
			Console.WriteLine("Process finished " + process.ExitCode);
			return true;
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