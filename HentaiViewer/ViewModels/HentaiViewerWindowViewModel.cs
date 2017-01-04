using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropertyChanged;
using RestSharp;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class HentaiViewerWindowViewModel : IDisposable {
		public static HentaiViewerWindowViewModel Instance;
		public HentaiModel _hentai;
		public ObservableCollection<object> _imageObjects = new ObservableCollection<object>();

		private bool _isClosing;

		private List<object> _images;
		public HentaiViewerWindowViewModel(HentaiModel hentai) {
			Instance = this;
			_hentai = hentai;
			ImageObjects = new ReadOnlyObservableCollection<object>(_imageObjects);
			GetImagesCommand = new ActionCommand(async () => {
				FetchButtonVisibility = Visibility.Collapsed;
				PregressBarVisibility = Visibility.Visible;
				var links = await SelectSite(hentai);
				_images = links;
				PregressBarVisibility = Visibility.Collapsed;
				for (var i = 0; i < 9; i++) {
					if (_isClosing) break;
					_imageObjects.Add(new ImageModel { PageNumber = i, Source = links[i] });
					await Task.Delay(200);
				}
				_loaded = 9;
			});
			SaveImagesCommand = new ActionCommand((() => {
				if (SaveProgress == Visibility.Collapsed) {
					SaveImages();
				}
			}));
		}

		public ReadOnlyObservableCollection<object> ImageObjects { get; }

		public string Pages { get; set; } = "0 : 0";

		public ICommand GetImagesCommand { get; }
		public ICommand SaveImagesCommand { get; }

		public Visibility FetchButtonVisibility { get; set; } = Visibility.Visible;

		public Visibility PregressBarVisibility { get; set; } = Visibility.Collapsed;

		public Visibility SaveProgress { get; set; } = Visibility.Collapsed;

		public async void Dispose() {
			_isClosing = true;
			await Task.Delay(100);
			_imageObjects.Clear();
			await Task.Delay(100);
			GC.Collect();
		}

		private async Task<List<object>> SelectSite(HentaiModel hentai) {
			Tuple<List<object>, int> tpl;
			switch (hentai.Site) {
				case "Hentai.cafe":
					tpl = await Sites.HentaiCafe.CollectImagesTaskAsync(hentai);
					Pages = $"{tpl.Item1.Count} : {tpl.Item2}";
					return tpl.Item1;
				case "nHentai.net":
					tpl = await nHentai.CollectImagesTaskAsync(hentai);
					Pages = $"{tpl.Item1.Count} : {tpl.Item2}";
					return tpl.Item1;
				case "ExHentai.org":
					tpl = await ExHentai.CollectImagesTaskAsync(hentai);
					Pages = $"{tpl.Item1.Count} : {tpl.Item2}";
					return tpl.Item1;
			}
			return null;
		}

		private async void SaveImages() {
			var folder = Path.Combine(Directory.GetCurrentDirectory(), "Saves", _hentai.Site, MD5Converter.MD5Hash(_hentai.Title));
			
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
			SaveProgress = Visibility.Visible;
			for (var i = 0; i < _imageObjects.Count; i++) {
				if (_isClosing) break;
				var img = (ImageModel)ImageObjects[i];
				if (img.Source is string) {
					var client = new RestClient {BaseUrl = new Uri((string) img.Source)};
					var imgBytes = await client.ExecuteGetTaskAsync(new RestRequest());
					img.Source = ExHentai.BytesToBitmapImage(imgBytes.RawBytes);
				}
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create((BitmapSource) img.Source));
				using (var stream = new FileStream($"{Path.Combine(folder, $"{i}.png")}", FileMode.Create)) {
					encoder.Save(stream);
				}
			}
			var output = JsonConvert.SerializeObject(new InfoModel(_hentai, _imageObjects.Count), Formatting.Indented,
				new StringEnumConverter { CamelCaseText = true });
			
			File.WriteAllText(Path.Combine(folder, "INFO.json"), output);
			SaveProgress = Visibility.Collapsed;
		}
		private int _loaded = 0;
		private bool adding = false;
		public async void LoadMoreImages() {
			if (adding || _images==null) {return;}
			adding = true;
			for (var i = 0; i < _images.Count; i++) {
				if (i == 9 || _loaded == _images.Count) {
					break;
				}
				_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded] });
				_loaded++;
				await Task.Delay(100);
			}
			adding = false;
		}
	}
}