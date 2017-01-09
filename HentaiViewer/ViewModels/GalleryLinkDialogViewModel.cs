using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HentaiViewer.Models;
using HentaiViewer.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace HentaiViewer.ViewModels {
	[ImplementPropertyChanged]
	public class GalleryLinkDialogViewModel {
		public string Link { get; set; }

		public ICommand ViewCommand { get; }

		public GalleryLinkDialogViewModel() {
			ViewCommand = new ActionCommand(View);
		}

		private void View() {
			var hm = new HentaiModel {
				Link = Link,
				Title = "lul"
			};
			if (Link.ToLower().Contains("exhentai")) {
				hm.Site = "ExHentai.org";
			}else if (Link.ToLower().Contains("nhentai")) {
				hm.Site = "nHentai.net";
			}else if (Link.ToLower().Contains("hentai.cafe")) {
				hm.Site = "Hentai.cafe";
			}
			else {
				return;
			}
			var viewWindow = new HentaiViewerWindow { DataContext = new HentaiViewerWindowViewModel(hm), WindowStartupLocation = WindowStartupLocation.CenterScreen };
			viewWindow.Show();
		}
	}
}
