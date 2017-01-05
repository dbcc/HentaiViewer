using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for FavoritesView.xaml
	/// </summary>
	public partial class FavoritesView : UserControl {
		public FavoritesView() {
			InitializeComponent();
			DataContext = new FavoritesViewModel();
		}
	}
}