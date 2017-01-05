using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for CafeView.xaml
	/// </summary>
	public partial class CafeView : UserControl {
		public static CafeView Instance;

		public CafeView() {
			Instance = this;
			InitializeComponent();
			DataContext = new HentaiCafeViewModel();
		}
	}
}