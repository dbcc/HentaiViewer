using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for nHentaiView.xaml
	/// </summary>
	public partial class nHentaiView : UserControl {
		public nHentaiView() {
			InitializeComponent();
			DataContext = new nHentaiViewModel();
		}
	}
}