using System;
using System.Diagnostics;
using System.IO;

namespace TestAgent
{
    public class BuildService
    {
        public void Build()
        {
	        var workingDirectory = AgentHelpers.GetWorkingDirectory();
	        var buildScriptPath = Path.Combine(workingDirectory, "buildTests.bat");

            var arguments = $"/c {buildScriptPath}";
            var processInfo = new ProcessStartInfo("cmd.exe", arguments)
            {
	            WorkingDirectory = workingDirectory
            };
            var process = Process.Start(processInfo);
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new Exception($"cmd exit code: [{process.ExitCode}], arguments: [{arguments}]");
        }
    }
}