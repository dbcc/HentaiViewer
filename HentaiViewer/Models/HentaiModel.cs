using System.IO;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;
using HentaiViewer.Views;
using PropertyChanged;

namespace HentaiViewer.Models {
	[ImplementPropertyChanged]
	public class HentaiModel {
		public HentaiModel() {
			MarkasReadCommand = new ActionCommand(() => Mark());
			ViewCommand = new ActionCommand(View);
		}

		public string Link { get; set; }
		public object Img { get; set; }
		public string Title { get; set; }
		public string Site { get; set; }
		public bool Seen { get; set; }
		public string Md5 => MD5Converter.MD5Hash(Title);

		public string SavePath => Path.Combine(Directory.GetCurrentDirectory(), "Saves", Site, Md5);

		public ICommand MarkasReadCommand { get; }
		public ICommand ViewCommand { get; }

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
			//todo save to a file if it's read
		}

		private void View() {
			//if (Site == "exhentai") return;
			var viewWindow = new HentaiViewerWindow {DataContext = new HentaiViewerWindowViewModel(this)};
			viewWindow.Show();
			Mark(false);
		}
	}
}