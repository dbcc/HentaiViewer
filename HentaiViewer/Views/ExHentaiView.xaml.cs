using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for ExHentaiView.xaml
	/// </summary>
	public partial class ExHentaiView : UserControl {
		public ExHentaiView() {
			InitializeComponent();
			DataContext = new ExHentaiViewModel();
		}
	}
}