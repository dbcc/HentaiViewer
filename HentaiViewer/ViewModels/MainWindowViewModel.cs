using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.Models;
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
			CheckUpdate();
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

		private async void CheckUpdate() {
			IsUpdateAvailable = await GithubController.CheckForUpdate();
		}

		private void OpenUpdateLink() {
			if (string.IsNullOrEmpty(GithubController.GithubUrl)) {
				return;
			}
			Process.Start(GithubController.GithubUrl);
		}

		private bool _dialogIsOpen = false;
		private async void OpenDialog() {
			if (_dialogIsOpen) {
				return;
			}
			_dialogIsOpen = true;
			var dia = new GalleryLinkDialog();
			await DialogHost.Show(dia);
			_dialogIsOpen = false;
		}
	}
}