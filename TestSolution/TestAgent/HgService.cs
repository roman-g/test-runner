using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestAgent
{
    public class HgService
    {
        private const string CheckoutDirectory = "work";
        public void CloneOrUpdate(string server, string branch)
        {
            //naive
            if (Directory.Exists(Path.Combine(AppContext.BaseDirectory, CheckoutDirectory)))
                Update(branch);
            else
                Clone(server, branch);
        }

        private void Clone(string server, string branch)
        {
            Console.Out.WriteLine("Starting clone...");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "hg",
                    Arguments = $"clone -b {branch} {server} {CheckoutDirectory}",
                    WorkingDirectory = AppContext.BaseDirectory
                }
            };
            process.Start();
            process.WaitForExit();
            Console.Out.WriteLine("Clone finished...");
        }

        private void Update(string branch)
        {
            Console.Out.WriteLine("Starting pull...");
            var pullProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "hg",
                    Arguments = $"pull",
                    WorkingDirectory = Path.Combine(AppContext.BaseDirectory, CheckoutDirectory)
                }
            };
            pullProcess.Start();
            pullProcess.WaitForExit();
            Console.Out.WriteLine("Starting update...");
            var updateProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "hg",
                    Arguments = $"pull",
                    WorkingDirectory = Path.Combine(AppContext.BaseDirectory, CheckoutDirectory)
                }
            };
            updateProcess.Start();
            updateProcess.WaitForExit();
            Console.Out.WriteLine("Update finished...");
        }
    }
}