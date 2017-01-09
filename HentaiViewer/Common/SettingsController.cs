using System;
using System.IO;
using HentaiViewer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HentaiViewer.Common {
	public static class SettingsController {
		private static readonly string ConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "Settings.json");
		public static SettingsModel Settings { get; set; }

		public static SettingsModel Load() {
			if (File.Exists(ConfigFile)) {
				var input = File.ReadAllText(ConfigFile);

				var jsonSettings = new JsonSerializerSettings();
				//jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
				//jsonSettings.NullValueHandling = NullValueHandling;
				jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
				//jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;
				Settings = JsonConvert.DeserializeObject<SettingsModel>(input, jsonSettings);
			}
			else {
				Settings = new SettingsModel {
					Cafe = new HentaiCafeOption(),
					ExHentai = new ExhentaiOption(),
					nHentai = new nHentaiOption()
				};
			}
			Save();
			return Settings;
		}

		public static void Save() {
			var output = JsonConvert.SerializeObject(Settings, Formatting.Indented,
				new StringEnumConverter {CamelCaseText = true});

			var folder = Path.GetDirectoryName(ConfigFile);
			if (folder != null && !Directory.Exists(folder)) Directory.CreateDirectory(folder);
			try {
				File.WriteAllText(ConfigFile, output);
			}
			catch (Exception) {
				//ignore
			}
		}
	}
}