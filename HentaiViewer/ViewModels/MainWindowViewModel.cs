using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using HentaiViewer.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class MainWindowViewModel {
		/// <summary>
		///     Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainWindowViewModel() {
			OpenLinkCommand = new ActionCommand(OpenDialog);
			UpdateCommand = new ActionCommand(OpenUpdateLink);
			CheckUpdateAsync();
		}

		public IEnumerable<object> Sites
			=>
				new List<object> {
					"Home",
					new Separator(),
					"ExHentai",
					"nHentai",
					"Cafe",
					new Separator(),
					"Favorites",
					"Saved Galleries"
				};

		public int SelectedSite { get; set; }
		public bool IsUpdateAvailable { get; set; }

		public ICommand OpenLinkCommand { get; }

		public ICommand UpdateCommand { get; }

		private async void CheckUpdateAsync() {
			IsUpdateAvailable = await GithubController.CheckForUpdateAsync();
		}

		private void OpenUpdateLink() {
			if (string.IsNullOrEmpty(GithubController.GithubUrl)) {
				return;
			}
			Process.Start(GithubController.GithubUrl);
		}

		//private bool _dialogIsOpen = false;
		private void OpenDialog() {
			//if (_dialogIsOpen) {
			//	return;
			//}
			//_dialogIsOpen = true;
			//var dia = new GalleryLinkDialog();
			//await DialogHost.Show(dia);
			//_dialogIsOpen = false;

			if (!Clipboard.ContainsText() && !Clipboard.GetText().StartsWith("http")) { return; }
			var link = Clipboard.GetText();
			var hm = new HentaiModel {
				Link = Clipboard.GetText(),
				Title = "lul"
			};
			if (link.ToLower().Contains("hentai.org/g/")) {
				hm.Site = "ExHentai.org";
			} else if (link.ToLower().Contains("nhentai.net/g/")) {
				hm.Site = "nHentai.net";
			} else if (link.ToLower().Contains("hentai.cafe")) {
				hm.Site = "Hentai.cafe";
			} else if (link.ToLower().Contains("pururin.us/gallery/")) {
				hm.Site = "Pururin.us";
			} else {
				return;
			}
			var viewWindow = new HentaiViewerWindow { DataContext = new HentaiViewerWindowViewModel(hm), WindowStartupLocation = WindowStartupLocation.CenterScreen };
			viewWindow.Show();
		}
	}
}