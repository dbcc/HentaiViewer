using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
    /// <summary>
    ///     Interaction logic for GalleryLinkDialog.xaml
    /// </summary>
    public partial class GalleryLinkDialog : UserControl {
        public GalleryLinkDialog() {
            InitializeComponent();
            DataContext = new GalleryLinkDialogViewModel();
        }
    }
}