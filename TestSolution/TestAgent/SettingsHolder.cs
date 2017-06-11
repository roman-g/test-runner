using System.IO;
using Newtonsoft.Json;

namespace TestAgent
{
	public class SettingsHolder
	{
		public Settings Settings { get; private set; }

		public SettingsHolder()
		{
			Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));
		}
	}
}