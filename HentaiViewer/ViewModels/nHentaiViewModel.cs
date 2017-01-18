using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using HentaiViewer.Views;
using PropertyChanged;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class nHentaiViewModel {
		public static nHentaiViewModel Instance;

		private readonly ObservableCollection<HentaiModel> _nHentai = new ObservableCollection<HentaiModel>();

		public nHentaiViewModel() {
			Instance = this;
			NHentaiItems = new ReadOnlyObservableCollection<HentaiModel>(_nHentai);

			//RefreshnHentaiAsync();
			RefreshnHentaiCommand = new ActionCommand(RefreshnHentaiAsync);
			LoadMorenHentaiCommand = new ActionCommand(async () => { await LoadnHentaiPageAsync(1); });
			LoadPrevnHentaiCommand = new ActionCommand(async () => {
				if (nHentaiLoadedPage == 1) return;
				await LoadnHentaiPageAsync(-1);
			});
			HomeCommand = new ActionCommand(async () => {
				if (nHentaiPageLoading) return;
				nHentaiLoadedPage = 1;
				NextnHentaiPage = 2;
				await LoadnHentaiPageAsync(0);
			});
			Setting = SettingsController.Settings;
		}

		public SettingsModel Setting { get; set; }

		public int nHentaiLoadedPage { get; set; } = 1;
		public int NextnHentaiPage { get; set; } = 2;
		public bool nHentaiPageLoading { get; set; }
		public ReadOnlyObservableCollection<HentaiModel> NHentaiItems { get; }

		public ICommand RefreshnHentaiCommand { get; }
		public ICommand LoadMorenHentaiCommand { get; }
		public ICommand LoadPrevnHentaiCommand { get; }

		public List<string> SortItems => new List<string> {"Date", "Popular"};

		public int SelectedSort { get; set; } = 0;

		public ICommand HomeCommand { get; }


		private async Task LoadnHentaiPageAsync(int value, bool delete = true) {
			SettingsController.Save();
			if (nHentaiPageLoading) return;
			nHentaiPageLoading = true;
			NextnHentaiPage = NextnHentaiPage + value;
			nHentaiLoadedPage = nHentaiLoadedPage + value;
			if (_nHentai.Count > 0 && delete) _nHentai.Clear();
			nHentaiView.Instance.ScrollViewer.ScrollToTop();
			var searchquery = SettingsController.Settings.nHentai.SearchQuery;
			List<HentaiModel> i;
			if (string.IsNullOrEmpty(searchquery))
				i = await nHentai.GetLatestAsync($"https://nhentai.net/?page={nHentaiLoadedPage}");
			else
				i = await nHentai.GetLatestAsync(
					$"https://nhentai.net/search/?q={searchquery.Replace(" ", "+")}&sort={SortItems[SelectedSort].ToLower()}&page={nHentaiLoadedPage}");
			foreach (var hentaiModel in i) {
				if (FavoritesController.FavoriteMd5s.Contains(hentaiModel.Md5)) hentaiModel.Favorite = true;
				_nHentai.Add(hentaiModel);
				await Task.Delay(10);
			}
			nHentaiPageLoading = false;
		}

		private async void RefreshnHentaiAsync() {
			await LoadnHentaiPageAsync(0);
		}
	}
}