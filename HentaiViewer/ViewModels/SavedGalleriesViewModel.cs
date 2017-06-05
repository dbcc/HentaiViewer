using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HentaiViewer.Models;
using Newtonsoft.Json;

namespace HentaiViewer.ViewModels {
    public class SavedGalleriesViewModel {
        private readonly ObservableCollection<HentaiModel> _galleries = new ObservableCollection<HentaiModel>();

        public SavedGalleriesViewModel() {
            Galleries = new ReadOnlyObservableCollection<HentaiModel>(_galleries);
            GetGalleries();
        }

        public ReadOnlyObservableCollection<HentaiModel> Galleries { get; }

        private void GetGalleries() {
            if (!Directory.Exists("Saves")) return;
            var folder = Directory.GetDirectories("Saves");
            foreach (var f in folder) {
                var galleries = Directory.GetDirectories(f);
                foreach (var gallery in galleries) {
                    var files = Directory.GetFiles(gallery);
                    var info = files.FirstOrDefault(fi => fi.Contains("INFO.json"));
                    if (info == null) continue;
                    var input = File.ReadAllText(info);

                    var jsonSettings = new JsonSerializerSettings {
                        ObjectCreationHandling = ObjectCreationHandling.Replace
                    };
                    var hen = JsonConvert.DeserializeObject<InfoModel>(input, jsonSettings);
                    _galleries.Add(new HentaiModel {
                        Link = hen.Link,
                        Img =
                            new Uri(Path.Combine(Directory.GetCurrentDirectory(), files[0])),
                        ThumbnailLink =
                            Path.Combine(Directory.GetCurrentDirectory(), files[0]),
                        Site = hen.Site,
                        Title = hen.Title,
                        IsSavedGallery = false
                    });
                }
            }
        }
    }
}