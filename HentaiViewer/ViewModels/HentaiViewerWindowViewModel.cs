using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using HentaiViewer.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropertyChanged;
using RestSharp;
using HentaiCafe = HentaiViewer.Sites.HentaiCafe;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class HentaiViewerWindowViewModel : IDisposable {
		public static HentaiViewerWindowViewModel Instance;
		private readonly HentaiModel _hentai;
		private readonly ObservableCollection<object> _imageObjects = new ObservableCollection<object>();
		private bool _adding;

		private List<object> _images;

		private bool _isClosing;

		public int _loaded { get; set; }

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
				for (var i = 0; i < links.Count; i++) {
					if (_isClosing || i == 9) break;
					_imageObjects.Add(new ImageModel {PageNumber = i, Source = links[i]});
					await Task.Delay(200);
					_loaded++;
				}
			});
			SaveImagesCommand = new ActionCommand(() => {
				if (SaveProgress == Visibility.Collapsed) SaveImages();
			});
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
					tpl = await HentaiCafe.CollectImagesTaskAsync(hentai);
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
			for (var i = 0; i < _images.Count; i++) {
				if (_isClosing) break;
				var img = _images[i];
				if (img is string) {
					var client = new RestClient {BaseUrl = new Uri((string) img)};
					var imgBytes = await client.ExecuteGetTaskAsync(new RestRequest());
					img = ExHentai.BytesToBitmapImage(imgBytes.RawBytes);
				}
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create((BitmapSource) img));
				using (var stream = new FileStream($"{Path.Combine(folder, $"{i}.png")}", FileMode.Create)) {
					encoder.Save(stream);
				}
			}
			var output = JsonConvert.SerializeObject(new InfoModel(_hentai, _images.Count), Formatting.Indented,
				new StringEnumConverter {CamelCaseText = true});

			File.WriteAllText(Path.Combine(folder, "INFO.json"), output);
			SaveProgress = Visibility.Collapsed;
		}
		public async Task LoadMoreImages() {
			if (_adding || _images == null) return;
			_adding = true;
			if (_imageObjects.Count >= 100) {
				_imageObjects.Clear();
				//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded-3] });
				//await Task.Delay(100);
				//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded-2] });
				//await Task.Delay(100);
				_imageObjects.Add(new ImageModel { PageNumber = _loaded-1, Source = _images[_loaded-1] });
				await Task.Delay(100);
			}
			for (var i = 0; i < _images.Count; i++) {
				if (_loaded == _images.Count || i == 9) break;
				_imageObjects.Add(new ImageModel {PageNumber = _loaded, Source = _images[_loaded]});
				_loaded++;
				await Task.Delay(100);
			}
			_adding = false;
		}

		//public async Task ImageLoader(bool reverse = false, HentaiViewerWindow win=null) {
		//	if (_adding || _images == null) return;
		//	_adding = true;
		//	if (!reverse) {
		//		if (_imageObjects.Count >= 9) {
		//			_imageObjects.Clear();
		//			//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded - 3] });
		//			//await Task.Delay(100);
		//			//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded - 2] });
		//			//await Task.Delay(100);
		//			_imageObjects.Add(new ImageModel { PageNumber = _loaded-1, Source = _images[_loaded - 1] });
		//			await Task.Delay(100);
		//		}
		//		for (var i = 0; i < _images.Count; i++) {
		//			if (_loaded == _images.Count || i == 9 || currentImages.Contains(_loaded)) break;
		//			_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded] });
		//			currentImages[i] = _loaded;
		//			_loaded++;
		//			await Task.Delay(100);
		//		}
		//	}
		//	else {
		//		var endpoint = _loaded - 9;
		//		var i = _loaded - 18;
		//		if (_imageObjects.Count >= 9) {
		//			_imageObjects.Clear();
		//			//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded - 3] });
		//			//await Task.Delay(100);
		//			//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded - 2] });
		//			//await Task.Delay(100);
		//			_imageObjects.Insert(0, new ImageModel { PageNumber = endpoint+1, Source = _images[endpoint+1] });
		//			await Task.Delay(100);
		//		}
		//		for (var startpoint = endpoint; startpoint > i; startpoint--) {
		//			if (startpoint == -1|| endpoint<=0) break;
		//			_imageObjects.Insert(0,new ImageModel { PageNumber = startpoint, Source = _images[startpoint] });
		//			//_imageObjects.RemoveAt(_imageObjects.Count-1);
		//			//currentImages[i] = ll;
		//			await Task.Delay(50);
		//		}
		//		if (endpoint>=9) {
		//			_loaded = endpoint;
		//		}
		//		//if (_imageObjects.Count >9 || _loaded >9) {

		//		//	for (var j = _imageObjects.Count; j > 9; j--) {
		//		//		if (_imageObjects.Count <=9) {
		//		//			break;
		//		//		}
		//		//		_imageObjects.RemoveAt(j-1);
		//		//	}
		//		//}
		//	}
		//	_adding = false;
		//}
	}
}