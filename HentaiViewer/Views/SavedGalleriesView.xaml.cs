using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
    /// <summary>
    ///     Interaction logic for SavedGalleriesView.xaml
    /// </summary>
    public partial class SavedGalleriesView : UserControl {
        public SavedGalleriesView() {
            InitializeComponent();
            DataContext = new SavedGalleriesViewModel();
        }
    }
}