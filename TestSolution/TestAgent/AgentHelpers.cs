using System;
using System.IO;

namespace TestAgent
{
	public static class AgentHelpers
	{
		public static string GetWorkingDirectory() => Path.Combine(AppContext.BaseDirectory, AgentConstants.WorkingDirectory);
	}
}