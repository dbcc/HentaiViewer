using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
    /// <summary>
    ///     Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : UserControl {
        public HistoryView() {
            InitializeComponent();
            DataContext = new HistoryViewModel();
        }
    }
}