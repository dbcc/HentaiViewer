using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for PururinView.xaml
	/// </summary>
	public partial class PururinView : UserControl {
		public static PururinView Instance;

		public PururinView() {
			InitializeComponent();
			Instance = this;
			DataContext = new PururinViewModel();
		}
	}
}