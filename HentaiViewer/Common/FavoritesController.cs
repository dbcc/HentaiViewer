using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HentaiViewer.Models;
using Newtonsoft.Json;

namespace HentaiViewer.Common {
    public class FavoritesController {
        private static readonly string ConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "Favorites.json");
        public static List<HentaiModel> Favorites { get; set; }

        public static List<string> FavoriteMd5s => Favorites.Select(f => f.Md5).ToList();

        public static List<HentaiModel> Load() {
            if (File.Exists(ConfigFile)) {
                var input = File.ReadAllText(ConfigFile);

                var jsonSettings = new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    ContractResolver = new EncryptedStringPropertyResolver("big-secret-memes")
                };
                Favorites = JsonConvert.DeserializeObject<List<HentaiModel>>(input, jsonSettings);
            }
            else {
                Favorites = new List<HentaiModel>();
            }
            Save();
            return Favorites;
        }

        public static void Save() {
            var jsonSettings = new JsonSerializerSettings {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ContractResolver = new EncryptedStringPropertyResolver("big-secret-memes")
            };
            var output = JsonConvert.SerializeObject(Favorites, Formatting.Indented, jsonSettings);

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