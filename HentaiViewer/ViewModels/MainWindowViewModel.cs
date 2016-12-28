using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using HentaiViewer.Models;
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
		}

		public ReadOnlyObservableCollection<HentaiModel> ExHentaiItems { get; }

		public IEnumerable<object> Sites => new List<object> {"Home", new Separator(), "ExHentai", "nHentai", "Cafe", new Separator(), "Favorites", "Saved Galleries" };

		public int SelectedSite { get; set; }
		//				break;
		//				ExHentaiViewModel.Instance.RefreshExHentaiCommand.Execute(null);
		//			case 0:
		//		switch (siteindex) {

		//private void LoadContent(int siteindex) {
		//			case 1:
		//				nHentaiViewModel.Instance.RefreshnHentaiCommand.Execute(null);
		//				break;
		//			case 2:
		//				HentaiCafeViewModel.Instance.RefreshCafeCommand.Execute(null);
		//				break;
		//		}
		//}
	}
}