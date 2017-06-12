using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestAgent
{
    public class HgService
    {
        public void CloneOrUpdate(string server, string branch)
        {
            //naive
            if (Directory.Exists(AgentHelpers.GetWorkingDirectory()))
                Update(branch);
            else
                Clone(server, branch);
        }

        private void Clone(string server, string branch)
        {
            Console.Out.WriteLine("Starting clone...");
            Run($"clone -b {branch} {server} {AgentConstants.WorkingDirectory}", AppContext.BaseDirectory);
            Console.Out.WriteLine("Clone finished...");
        }

        private void Update(string branch)
        {
            Console.Out.WriteLine("Starting pull...");
            Run($"pull", AgentHelpers.GetWorkingDirectory());
            Console.Out.WriteLine("Starting update...");
            Run($"update {branch}", AgentHelpers.GetWorkingDirectory());
            Console.Out.WriteLine("Update finished...");
        }

        private static void Run(string arguments, string workingDirectory)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "hg",
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory
                }
            };
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new Exception($"hg exit code: [{process.ExitCode}], arguments: [{arguments}], workingDirectory: [{workingDirectory}]");
        }
    }
}