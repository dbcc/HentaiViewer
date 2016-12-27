using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for CafeView.xaml
	/// </summary>
	public partial class CafeView : UserControl {
		public CafeView() {
			InitializeComponent();
			DataContext = new HentaiCafeViewModel();
		}
	}
}