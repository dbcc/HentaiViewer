using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using HentaiViewer.Models;
using HentaiViewer.Sites;
using PropertyChanged;
using HentaiCafe = HentaiViewer.Sites.HentaiCafe;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class HentaiCafeViewModel {
		public static HentaiCafeViewModel Instance;

		private readonly ObservableCollection<HentaiModel> _cafe = new ObservableCollection<HentaiModel>();

		public HentaiCafeViewModel() {
			Instance = this;
			CafeItems = new ReadOnlyObservableCollection<HentaiModel>(_cafe);

			RefreshCafeAsync();
			RefreshCafeCommand = new ActionCommand(RefreshCafeAsync);
			LoadMoreCafeCommand = new ActionCommand(async () => {
				CafeLoadedPage++;
				await LoadCafePage();
			});
			LoadPrevCafeCommand = new ActionCommand(async () => {
				if (CafeLoadedPage == 1) return;
				CafeLoadedPage--;
				await LoadCafePage();
			});
		}

		public int CafeLoadedPage { get; set; } = 1;
		public int NextCafePage { get; set; } = 2;
		public bool CafePageLoading { get; set; }
		public ReadOnlyObservableCollection<HentaiModel> CafeItems { get; }

		public ICommand RefreshCafeCommand { get; }
		public ICommand LoadMoreCafeCommand { get; }
		public ICommand LoadPrevCafeCommand { get; }

		private async void RefreshCafeAsync() {
			await LoadCafePage();
		}

		private async Task LoadCafePage() {
			if (CafePageLoading) return;
			CafePageLoading = true;
			NextCafePage = CafeLoadedPage + 1;
			if (_cafe.Count > 0) _cafe.Clear();
			var i = await HentaiCafe.GetLatest($"https://hentai.cafe/page/{CafeLoadedPage}");
			foreach (var hentaiModel in i) {
				_cafe.Add(hentaiModel);
				await Task.Delay(100);
			}
			CafePageLoading = false;
		}
	}
}