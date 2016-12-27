using System;
using System.Collections.Generic;
using System.IO;
using HentaiViewer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HentaiViewer.Common {
	internal class HistoryController {
		private static readonly string ConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "History.json");
		public static HistoryModel History { get; set; }

		public static HistoryModel Load() {
			if (File.Exists(ConfigFile)) {
				var input = File.ReadAllText(ConfigFile);

				var jsonSettings = new JsonSerializerSettings();
				jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
				jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
				jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;
				History = JsonConvert.DeserializeObject<HistoryModel>(input, jsonSettings);
			}
			else {
				History = new HistoryModel {Items = new List<string>()};
			}
			Save();
			return History;
		}

		public static void Save() {
			var output = JsonConvert.SerializeObject(History, Formatting.Indented,
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

		public static bool CheckHistory(string title) {
			return History.Items.Contains(title);
		}
	}
}