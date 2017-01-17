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

				var jsonSettings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};
				Settings = JsonConvert.DeserializeObject<SettingsModel>(input, jsonSettings);
				if (Settings.ExHentai == null) Settings.ExHentai = new ExhentaiOption();
				if (Settings.nHentai == null) Settings.nHentai = new nHentaiOption();
				if (Settings.Cafe == null) Settings.Cafe = new HentaiCafeOption();
				if (Settings.Pururin == null) Settings.Pururin = new PururinOption();
			}
			else {
				Settings = new SettingsModel {
					Cafe = new HentaiCafeOption(),
					ExHentai = new ExhentaiOption(),
					nHentai = new nHentaiOption(),
					Pururin = new PururinOption()
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