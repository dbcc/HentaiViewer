using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using RestSharp;

namespace HentaiViewer.ViewModels {
	public class FavoritesViewModel {
		public ObservableCollection<HentaiModel> _favorites = new ObservableCollection<HentaiModel>();
		public ReadOnlyObservableCollection<HentaiModel> FavoriteItems { get; }

		public static FavoritesViewModel Instance;

		public FavoritesViewModel() {
			Instance = this;
			FavoriteItems = new ReadOnlyObservableCollection<HentaiModel>(_favorites);
			foreach (var hentaiModel in FavoritesController.Favorites) {
				if (hentaiModel.Site == "ExHentai.org") {
					var client = new RestClient {
						UserAgent =
							"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.70 Safari/537.36",
						Encoding = Encoding.UTF8,
						Timeout = 60000,
						BaseUrl = new Uri(hentaiModel.ThumbnailLink),
						CookieContainer = ExHentai.GetCookies()
					};
					var imgbytes = client.Execute(new RestRequest());
					try {
						var img = ExHentai.BytesToBitmapImage(imgbytes.RawBytes);
						hentaiModel.Img = img;
					}
					catch {//ignore
					}
				}
				else {
					hentaiModel.Img = hentaiModel.ThumbnailLink;
				}
				_favorites.Add(hentaiModel);
			}
		}
	}
}
