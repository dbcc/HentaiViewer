using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using HentaiViewer.Models;
using HentaiViewer.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class MainWindowViewModel {
		private readonly ObservableCollection<HentaiModel> _exHentai = new ObservableCollection<HentaiModel>();

		/// <summary>
		///     Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainWindowViewModel() {
			//ExHentai.GetLatest("k");
			ExHentaiItems = new ReadOnlyObservableCollection<HentaiModel>(_exHentai);
			//SelectedSite = 0;
			OpenLinkCommand = new ActionCommand(OpenDialog);
		}

		public ReadOnlyObservableCollection<HentaiModel> ExHentaiItems { get; }

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

		public ICommand OpenLinkCommand { get; }
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