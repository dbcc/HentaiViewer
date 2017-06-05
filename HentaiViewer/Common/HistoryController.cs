using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HentaiViewer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HentaiViewer.Common {
    internal class HistoryController {
        private static readonly string ConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "History.json");
        public static ObservableCollection<HistoryModel> History { get; set; }

        public static ObservableCollection<HistoryModel> Load() {
            if (File.Exists(ConfigFile)) {
                var input = File.ReadAllText(ConfigFile);

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;
                jsonSettings.ContractResolver = new EncryptedStringPropertyResolver("big-secret-memes");
                History = JsonConvert.DeserializeObject<ObservableCollection<HistoryModel>>(input, jsonSettings);
            }
            else {
                History = new ObservableCollection<HistoryModel>();
            }
            Save();
            return History;
        }

        public static void Save() {
            var jsonSettings = new JsonSerializerSettings {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ContractResolver = new EncryptedStringPropertyResolver("big-secret-memes"),
                Formatting = Formatting.Indented
            };

            jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
            var output = JsonConvert.SerializeObject(History, jsonSettings);

            var folder = Path.GetDirectoryName(ConfigFile);
            if (folder != null && !Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            try {
                File.WriteAllText(ConfigFile, output);
            }
            catch (Exception) {
                //ignore
            }
        }

        public static bool CheckHistory(string link) {
            return History.Any(h => h.Link == link);
        }
    }
}