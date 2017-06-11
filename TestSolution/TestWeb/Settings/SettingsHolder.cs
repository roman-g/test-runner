using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace TestWeb.Settings
{
	public class SettingsHolder
	{
		public WebSettings AgentSettings { get; private set; }

		public SettingsHolder()
		{
			var directory = AppContext.BaseDirectory;
			AgentSettings = JsonConvert.DeserializeObject<WebSettings>(File.ReadAllText(Path.Combine(directory, "settings.json")));
		}
	}
}