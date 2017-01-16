using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using HentaiViewer.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
//using HentaiCafe = HentaiViewer.Sites.HentaiCafe;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class HentaiCafeViewModel {
		public static HentaiCafeViewModel Instance;

		private readonly ObservableCollection<HentaiModel> _cafe = new ObservableCollection<HentaiModel>();

		public HentaiCafeViewModel() {
			Instance = this;
			CafeItems = new ReadOnlyObservableCollection<HentaiModel>(_cafe);

			//RefreshCafeAsync();
			RefreshCafeCommand = new ActionCommand(RefreshCafeAsync);
			LoadMoreCafeCommand = new ActionCommand(async () => { await LoadCafePage(1); });
			LoadPrevCafeCommand = new ActionCommand(async () => {
				if (CafeLoadedPage == 1) return;
				await LoadCafePage(-1);
			});
			HomeCommand = new ActionCommand(async () => {
				if (CafePageLoading) return;
				CafeLoadedPage = 1;
				NextCafePage = 2;
				await LoadCafePage(0);
			});
		}

		public int CafeLoadedPage { get; set; } = 1;
		public int NextCafePage { get; set; } = 2;
		public bool CafePageLoading { get; set; }
		public ReadOnlyObservableCollection<HentaiModel> CafeItems { get; }

		public ICommand RefreshCafeCommand { get; }
		public ICommand LoadMoreCafeCommand { get; }
		public ICommand LoadPrevCafeCommand { get; }

		public ICommand HomeCommand { get; }

		private async void RefreshCafeAsync() {
			await LoadCafePage(0);
		}

		private async Task LoadCafePage(int value) {
			if (CafePageLoading) return;
			CafePageLoading = true;
			NextCafePage = NextCafePage + value;
			CafeLoadedPage = CafeLoadedPage + value;
			if (_cafe.Count > 0) _cafe.Clear();
			CafeView.Instance.ScrollViewer.ScrollToTop();
			var searchquery = SettingsController.Settings.Cafe.SearchQuery;
			List<HentaiModel> i;
			if (string.IsNullOrEmpty(searchquery)) i = await HentaiCafe.GetLatestAsync($"https://hentai.cafe/page/{CafeLoadedPage}");
			else {
				 i = await HentaiCafe.GetLatestAsync($"https://hentai.cafe/page/{CafeLoadedPage}/?s={searchquery.Replace(" ", "+")}");
			}
			foreach (var hentaiModel in i) {
				if (FavoritesController.FavoriteMd5s.Contains(hentaiModel.Md5)) hentaiModel.Favorite = true;
				_cafe.Add(hentaiModel);
				await Task.Delay(10);
			}
			CafePageLoading = false;
		}
	}
}