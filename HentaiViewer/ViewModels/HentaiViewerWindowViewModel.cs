using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using Newtonsoft.Json;
using System.Linq;

namespace HentaiViewer.ViewModels
{
    public class HentaiViewerWindowViewModel : IDisposable, INotifyPropertyChanged
    {
        private readonly ObservableCollection<object> _imageObjects = new ObservableCollection<object>();
        private readonly ObservableCollection<int> _pagesList = new ObservableCollection<int>();
        private bool _adding;
        private ObservableCollection<object> _images = new ObservableCollection<object>();

        //private List<object> _images;

        private bool _isClosing;
        //private bool _saving;

        public HentaiViewerWindowViewModel(HentaiModel hentai, bool saveEnabled = true)
        {
            Hentai = hentai;
            CopyLinkCommand = new ActionCommand(() => Clipboard.SetText(Hentai.Link));
            ChangeModeCommand = new ActionCommand(ChangeMode);
            JumpCommand = new ActionCommand(Jump);
            ImageObjects = new ReadOnlyObservableCollection<object>(_imageObjects);
            GetImagesCommand = new ActionCommand(async () =>
            {
                FetchButtonVisibility = Visibility.Collapsed;
                PregressBarVisibility = Visibility.Visible;
                var links = new List<object>();
                if (!hentai.IsSavedGallery)
                {
                    var files = Directory.GetFiles(hentai.SavePath).ToList();
                    files.ForEach(links.Add);
                    files.Remove("INFO.json");
                    SetPages(files.Count, files.Count);
                }
                else
                {
                    links = await SelectSiteAsync(hentai);
                }

                _images = new ObservableCollection<object>(links);
                Images = new ReadOnlyObservableCollection<object>(_images);
                PagesList = new ReadOnlyObservableCollection<int>(_pagesList);
                PageIntList();
                PregressBarVisibility = Visibility.Collapsed;
                foreach (var link in links)
                {
                    if (_isClosing || _imageObjects.Count == 5)
                    {
                        break;
                    }
                    _imageObjects.Add(new ImageModel
                    {
                        PageNumber = links.IndexOf(link) + 1,
                        Source = link,
                        IsGif = link.ToString().Contains(".gif")
                    });
                    await Task.Delay(10);
                    Loaded++;
                }
                SaveEnabled = saveEnabled;
            });
            SaveImagesCommand = new ActionCommand(() =>
            {
                if (SaveProgress == Visibility.Collapsed)
                {
                    SaveImagesAsync();
                }
            });
            Setting = SettingsController.Settings;
            if (Setting.Other.InstantFetch)
            {
                GetImagesCommand.Execute(null);
            }
        }

        public int SelectedPage { get; set; }

        public int TransIndex { get; set; } = 1;

        public string Mode { get; set; } = "Singe Page";

        public ReadOnlyObservableCollection<int> PagesList { get; set; }

        public SettingsModel Setting { get; set; }

        public HentaiModel Hentai { get; set; }

        public int Loaded { get; private set; }
        public bool SaveEnabled { get; set; }

        public ReadOnlyObservableCollection<object> ImageObjects { get; }
        public ReadOnlyObservableCollection<object> Images { get; set; }

        public string Pages { get; set; } = "0 : 0";

        public ICommand GetImagesCommand { get; }
        public ICommand SaveImagesCommand { get; }
        public ICommand ChangeModeCommand { get; }

        public Visibility FetchButtonVisibility { get; set; } = Visibility.Visible;

        public Visibility PregressBarVisibility { get; set; } = Visibility.Collapsed;

        public Visibility SaveProgress { get; set; } = Visibility.Collapsed;

        public int ProgressValue { get; set; }

        public ICommand CopyLinkCommand { get; }

        public int JumpTonumber { get; set; }

        public ICommand JumpCommand { get; }

        public async void Dispose()
        {
            _isClosing = true;
            await Task.Delay(100);
            _imageObjects.Clear();
            await Task.Delay(100);
            GC.Collect();
        }
        //			_imageObjects.Insert(0,new ImageModel { PageNumber = startpoint, Source = _images[startpoint] });
        //			if (startpoint == -1|| endpoint<=0) break;
        //		for (var startpoint = endpoint; startpoint > i; startpoint--) {
        //		}
        //			await Task.Delay(100);
        //			_imageObjects.Insert(0, new ImageModel { PageNumber = endpoint+1, Source = _images[endpoint+1] });
        //			//await Task.Delay(100);
        //			//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded - 2] });
        //			//await Task.Delay(100);
        //			//_imageObjects.Add(new ImageModel { PageNumber = _loaded, Source = _images[_loaded - 3] });
        //			_imageObjects.Clear();
        //		if (_imageObjects.Count >= 9) {
        //		var i = _loaded - 18;

        //		}

        //		var endpoint = _loaded - 9;
        //	else {
        //	}

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
        public event PropertyChangedEventHandler PropertyChanged;

        private void ChangeMode()
        {
            if (TransIndex == 1)
            {
                TransIndex = 0;
                Mode = "Singe Page";
                return;
            }
            TransIndex = 1;
            Mode = "Long Strip";
        }

        private void PageIntList()
        {
            for (var i = 0; i < _images.Count; i++)
            {
                _pagesList.Add(i);
            }
        }

        private async void Jump()
        {
            _imageObjects?.Clear();
            Loaded = JumpTonumber;
            await LoadMoreImagesAsync();
        }

        private async Task<List<object>> SelectSiteAsync(HentaiModel hentai)
        {
            switch (hentai.Site)
            {
                case "Hentai.cafe":
                    {
                        var (paths, count) = await HentaiCafe.CollectImagesTaskAsync(hentai, SetPages);
                        Pages = $"{paths.Count} : {count}";
                        return paths;
                    }
                case "nHentai.net":
                    {
                        var (paths, count) = await NHentai.CollectImagesTaskAsync(hentai, SetPages);
                        Pages = $"{paths.Count} : {count}";
                        return paths;
                    }
                case "ExHentai.org":
                    {
                        var (paths, count) = await ExHentai.CollectImagesTaskAsync(hentai, SetPages);
                        Pages = $"{paths.Count} : {count}";
                        return paths;
                    }
                case "Pururin.us":
                    {
                        var (paths, count) = await Pururin.CollectImagesTaskAsync(hentai, SetPages);
                        Pages = $"{paths.Count} : {count}";
                        return paths;
                    }
                case "Imgur.com":
                    {
                        var (paths, count) = await Sites.Imgur.CollectImagesTaskAsync(hentai, SetPages);
                        Pages = $"{paths.Count} : {count}";
                        return paths;
                    }
                default:
                    return null;
            }
        }

        private void SetPages(int current, int max)
        {
            Pages = $"{current} : {max}";
        }

        private async void SaveImagesAsync()
        {
            if (Hentai.Title == "lul" || !SaveEnabled)
            {
                return;
            }
            SaveEnabled = false;
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Saves", Hentai.Site,
                MD5Converter.MD5Hash(Hentai.Title));

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            SaveProgress = Visibility.Visible;
            for (var i = 0; i < _images.Count; i++)
            {
                if (_isClosing)
                {
                    break;
                }
                var img = _images[i];
                ProgressValue = i + 1;
                await SaveImage(img.ToString(), i + 1, folder);
                await Task.Delay(50);
            }
            var output = JsonConvert.SerializeObject(new InfoModel(Hentai, _images.Count), Formatting.Indented);

            File.WriteAllText(Path.Combine(folder, "INFO.json"), output);
            SaveProgress = Visibility.Collapsed;
            SaveEnabled = true;
        }

        public async Task LoadMoreImagesAsync()
        {
            if (_adding || _images == null || _images.Count <= 20)
            {
                return;
            }
            _adding = true;
            if (_imageObjects.Count > 20)
            {
                for (var i = 0; i < 5; i++)
                {
                    _imageObjects.RemoveAt(i);
                    await Task.Delay(10);
                }
            }
            for (var i = 0; i < _images.Count; i++)
            {
                if (Loaded == _images.Count || i == 5)
                {
                    break;
                }
                _imageObjects.Add(new ImageModel
                {
                    PageNumber = Loaded,
                    Source = _images[Loaded],
                    IsGif = _images[Loaded].ToString().Contains(".gif")
                });
                Loaded++;
                await Task.Delay(10);
            }
            _adding = false;
        }

        private async Task SaveImage(string url, int num, string folder)
        {
            try
            {
                var lastSlash = url.LastIndexOf('/');
                var guid = url.Substring(lastSlash + 1);
                var client = new WebClient();
                var extension = guid.Substring(guid.LastIndexOf(".", StringComparison.Ordinal) + 1);
                await client.DownloadFileTaskAsync(url, Path.Combine(folder, $"{num}.{extension}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}