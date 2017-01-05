using System.Windows;
using System.Windows.Controls;

namespace HentaiViewer.Models {
	/// <summary>
	///     Interaction logic for ImageView.xaml
	/// </summary>
	public partial class ImageView : UserControl {
		public ImageView() {
			InitializeComponent();
		}

		private void Image_OnImageFailed(object sender, ExceptionRoutedEventArgs e) {
			//Image.Source = new BitmapImage(new Uri("/Resources/hmmm.jpg", UriKind.Relative));
		}
	}
}