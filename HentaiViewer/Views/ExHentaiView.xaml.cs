using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for ExHentaiView.xaml
	/// </summary>
	public partial class ExHentaiView : UserControl {
		public static ExHentaiView Instance;
		public ExHentaiView() {
			Instance = this;
			InitializeComponent();
			DataContext = new ExHentaiViewModel();
		}
	}
}