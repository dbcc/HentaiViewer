using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using HentaiViewer.Views;
using PropertyChanged;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class PururinViewModel {
		public static PururinViewModel Instance;

		private readonly ObservableCollection<HentaiModel> _Pururin = new ObservableCollection<HentaiModel>();

		public PururinViewModel() {
			Instance = this;
			PururinItems = new ReadOnlyObservableCollection<HentaiModel>(_Pururin);

			//RefreshPururinAsync();
			RefreshPururinCommand = new ActionCommand(RefreshPururinAsync);
			LoadMorePururinCommand = new ActionCommand(async () => { await LoadPururinPage(1); });
			LoadPrevPururinCommand = new ActionCommand(async () => {
				if (PururinLoadedPage == 1) return;
				await LoadPururinPage(-1);
			});
			HomeCommand= new ActionCommand(async () => {
				if (PururinPageLoading) { return; }
				PururinLoadedPage = 1;
				NextPururinPage = 2;
				await LoadPururinPage(0);
			});
		}

		public int PururinLoadedPage { get; set; } = 1;
		public int NextPururinPage { get; set; } = 2;
		public bool PururinPageLoading { get; set; }
		public ReadOnlyObservableCollection<HentaiModel> PururinItems { get; }

		public ICommand RefreshPururinCommand { get; }
		public ICommand LoadMorePururinCommand { get; }
		public ICommand LoadPrevPururinCommand { get; }
		private Dictionary<string, int> _filters = new Dictionary<string, int>{{"Newest",1}, {"Most Popular",2},{"Highest Rated",3}, {"Most Viewed",4}, {"Title",5}, {"Random",6}};

		public List<string> SortItems => _filters.Keys.ToList();
		public string SelectedFilter { get; set; } = "Newest";

		public ICommand HomeCommand { get; }


		private bool StopAction = false;
		private async Task LoadPururinPage(int value, bool delete = true) {
			if (PururinPageLoading) return;
			PururinPageLoading = true;
			NextPururinPage = NextPururinPage + value;
			PururinLoadedPage = PururinLoadedPage + value;
			if (_Pururin.Count > 0 && delete) _Pururin.Clear();
			PururinView.Instance.ScrollViewer.ScrollToTop();
			var searchquery = SettingsController.Settings.Pururin.SearchQuery;
			List<HentaiModel> i;
			if (string.IsNullOrEmpty(searchquery)) i = await Pururin.GetLatest($"http://pururin.us/browse/search/{_filters[SelectedFilter]}/{PururinLoadedPage}.html");
			else
				i = await Pururin.GetLatest(
					$"http://pururin.us/search/more?q={searchquery.Replace(" ", "+")}&p={PururinLoadedPage}");
			foreach (var hentaiModel in i) {
				if (FavoritesController.FavoriteMd5s.Contains(hentaiModel.Md5)) hentaiModel.Favorite = true;
				_Pururin.Add(hentaiModel);
				await Task.Delay(10);
			}
			PururinPageLoading = false;
		}

		private async void RefreshPururinAsync() {
			await LoadPururinPage(0);
		}
	}
}