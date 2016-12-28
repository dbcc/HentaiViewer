using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for nHentaiView.xaml
	/// </summary>
	public partial class nHentaiView : UserControl {
		public static nHentaiView Instance;
		public nHentaiView() {
			Instance = this;
			InitializeComponent();
			DataContext = new nHentaiViewModel();
		}
	}
}