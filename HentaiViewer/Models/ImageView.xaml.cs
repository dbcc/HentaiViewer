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

namespace HentaiViewer.Models {
	/// <summary>
	/// Interaction logic for ImageView.xaml
	/// </summary>
	public partial class ImageView : UserControl {
		public ImageView() {
			InitializeComponent();
		}

		private void Image_OnImageFailed(object sender, ExceptionRoutedEventArgs e) {
			((Image)sender).Source = new BitmapImage(new Uri("/Resources/hmmm.jpg", UriKind.Relative));
		}
	}
}
