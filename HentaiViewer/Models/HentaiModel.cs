using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;
using HentaiViewer.Views;
using Newtonsoft.Json;
using PropertyChanged;
using MessageBox = System.Windows.Forms.MessageBox;

namespace HentaiViewer.Models {
	[ImplementPropertyChanged]
	public class HentaiModel {
		public HentaiModel() {
			MarkasReadCommand = new ActionCommand(() => Mark());
			ViewCommand = new ActionCommand(View);
			FavoriteCommand = new ActionCommand(ToggleFavorite);
			OpenLinkCommand = new ActionCommand(() => {
				try {
					Process.Start(Link);
				}
				catch{
					//ignore
				}
			});
		}

		public string Link { get; set; }

		[JsonIgnore]
		public object Img { get; set; }

		public string Title { get; set; }
		public string Site { get; set; }
		public bool Seen { get; set; }
		public string ThumbnailLink { get; set; }

		public string Md5 => MD5Converter.MD5Hash(Title);

		public bool IsSavedGallery { get; set; } = true;

		public string SavePath => Path.Combine(Directory.GetCurrentDirectory(), "Saves", Site, Md5);

		[JsonIgnore]
		public ICommand MarkasReadCommand { get; }

		[JsonIgnore]
		public ICommand ViewCommand { get; }

		[JsonIgnore]
		public ICommand FavoriteCommand { get; }

		[JsonIgnore]
		public ICommand OpenLinkCommand { get; }

		public bool Favorite { get; set; }

		private void ToggleFavorite() {
			if (!IsSavedGallery) return;
			Favorite = !Favorite;
			if (Favorite) {
				if (!FavoritesController.FavoriteMd5s.Contains(Md5)) {
					FavoritesController.Favorites.Add(this);
					FavoritesViewModel.Instance._favorites.Add(this);
				}
			}
			else {
				foreach (var hentaiModel in FavoritesViewModel.Instance._favorites) {
					if (hentaiModel.Md5 != Md5) continue;
					FavoritesController.Favorites.Remove(hentaiModel);
					FavoritesViewModel.Instance._favorites.Remove(hentaiModel);
					break;
				}
			}
			FavoritesController.Save();
		}

		private void Mark(bool toggle = true) {
			if (!IsSavedGallery) return;
			if (toggle) Seen = !Seen;
			else Seen = true;
			if (Seen) {
				if (!HistoryController.CheckHistory(Title, Link)) HistoryController.History.Insert(0, new HistoryModel {
					Date = DateTime.Now,
					Title = Title,
					Link = Link,
					Site = Site
				});
			}
			else {
				HistoryController.History.ToList().RemoveAll(h=>h.Title == Title || h.Link == Link);
			}
			HistoryController.Save();
		}

		private void View() {
			if (Site == "ExHentai.org" && !IsSavedGallery) {
				var pass = SettingsController.Settings.ExHentai.IpbPassHash;
				var memid = SettingsController.Settings.ExHentai.IpbMemberId;
				var igneous = SettingsController.Settings.ExHentai.Igneous;
				if (string.IsNullOrEmpty(memid) || string.IsNullOrEmpty(igneous) || string.IsNullOrEmpty(pass)) {
					MessageBox.Show("Need Cookies for Exhentai", "Cookies missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			var viewWindow = new HentaiViewerWindow {
				DataContext = new HentaiViewerWindowViewModel(this),
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};
			viewWindow.Show();
			Mark(false);
		}
	}
}