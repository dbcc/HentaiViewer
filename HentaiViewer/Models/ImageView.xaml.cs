using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HentaiViewer.Models {
	/// <summary>
	///     Interaction logic for ImageView.xaml
	/// </summary>
	public partial class ImageView : UserControl {
		public ImageView() {
			InitializeComponent();
		}

		private void Image_OnImageFailed(object sender, ExceptionRoutedEventArgs e) {
			//try {
			//	var img = (Image) sender;
			//	img.Source = new BitmapImage(new Uri("/Resources/hmmm.jpg", UriKind.Relative));
			//}
			//catch {
			//	//ignore
			//}
		}
	}
}