using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	/// Interaction logic for SavedGalleriesView.xaml
	/// </summary>
	public partial class SavedGalleriesView : UserControl {
		public SavedGalleriesView() {
			InitializeComponent();
			DataContext = new SavedGalleriesViewModel();
		}
	}
}
