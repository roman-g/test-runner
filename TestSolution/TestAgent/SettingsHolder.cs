using System.IO;
using Newtonsoft.Json;

namespace TestAgent
{
	public class SettingsHolder
	{
		public AgentSettings AgentSettings { get; private set; }

		public SettingsHolder()
		{
			AgentSettings = JsonConvert.DeserializeObject<AgentSettings>(File.ReadAllText("settings.json"));
		}
	}
}