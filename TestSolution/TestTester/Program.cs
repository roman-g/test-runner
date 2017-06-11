using System;
using System.Linq;
using Akka.Actor;
using Akka.Configuration;
using TestCommon;

namespace TestTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    stdout-loglevel = DEBUG
    loglevel = DEBUG
    log-config-on-start = on        
    actor {                
        debug {  
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
		    port = 0
		    hostname = localhost
            maximum-frame-size = 4000000b
        }
    }
}
");
            using (var system = ActorSystem.Create("TestTester", config))
            {
                var starter = system.ActorOf<StartActor>("starter");
                starter.Tell(new Start());
                Console.ReadLine();
            }
        }
    }

    public class StartActor : ReceiveActor, ILogReceive
    {
        public StartActor()
        {
            Console.WriteLine("Starting StartActor");
            var server = Context.ActorSelection("akka.tcp://TestAgent@localhost:8081/user/Agent");
            Receive<Start>(start =>
            {
                var allTests = server.Ask<ParseResult>(new Parse()).Result.TestNames;
                var testsToRun = allTests.Take(3).ToArray();
                Console.Out.WriteLine("testsToRun = {0}", string.Join(",", testsToRun));
                server.Tell(new RunTests
                {
                    TestNames = testsToRun
                });

            });
            Receive<TestFinished>(testFinished =>
            {
                Console.Out.WriteLine($"Yay! TestFinished: {testFinished.TestName}, {testFinished.Result}");
            });
            Console.WriteLine("StartActor Started");
        }
    }


    public class Start
    {
        
    }
}
