using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;
using HentaiViewer.Views;
using Newtonsoft.Json;
using PropertyChanged;

namespace HentaiViewer.Models {
	[ImplementPropertyChanged]
	public class HentaiModel {
		public HentaiModel() {
			MarkasReadCommand = new ActionCommand(() => Mark());
			ViewCommand = new ActionCommand(View);
			FavoriteCommand = new ActionCommand(ToggleFavorite);
		}

		public string Link { get; set; }
		public object Img { get; set; }
		public string Title { get; set; }
		public string Site { get; set; }
		public bool Seen { get; set; }
		public string Md5 => MD5Converter.MD5Hash(Title);

		public string SavePath => Path.Combine(Directory.GetCurrentDirectory(), "Saves", Site, Md5);
		[JsonIgnore]
		public ICommand MarkasReadCommand { get; }
		[JsonIgnore]
		public ICommand ViewCommand { get; }
		[JsonIgnore]
		public ICommand FavoriteCommand { get; }

		public bool Favorite { get; set; }

		private void ToggleFavorite() {
			Favorite = !Favorite;
			if (Favorite) {
				if (!FavoritesController.FavoriteMd5s.Contains(Md5)) {
					FavoritesController.Favorites.Add(this);
					FavoritesViewModel.Instance._favorites.Add(this);
				}
			}
			else {
				foreach (var hentaiModel in FavoritesViewModel.Instance._favorites) {
					if (hentaiModel.Md5 == Md5) {
						FavoritesController.Favorites.Remove(hentaiModel);
						FavoritesViewModel.Instance._favorites.Remove(hentaiModel);
						break;
					}
				}
			}
			FavoritesController.Save();
		}

		private void Mark(bool toggle = true) {
			if (toggle) {Seen = !Seen;}
			else {
				Seen = true;
			}
			if (Seen) {
				if (!HistoryController.History.Items.Contains(Md5)) HistoryController.History.Items.Add(Md5);
			}
			else {
				HistoryController.History.Items.Remove(Md5);
			}
			HistoryController.Save();
		}

		private void View() {
			//if (Site == "exhentai") return;
			var viewWindow = new HentaiViewerWindow {DataContext = new HentaiViewerWindowViewModel(this)};
			viewWindow.Show();
			Mark(false);
		}
	}
}