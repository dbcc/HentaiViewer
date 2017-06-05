using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
    /// <summary>
    ///     Interaction logic for nHentaiView.xaml
    /// </summary>
    public partial class NHentaiView : UserControl {
        public static NHentaiView Instance;

        public NHentaiView() {
            Instance = this;
            InitializeComponent();
            DataContext = new NHentaiViewModel();
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) {
                return;
            }
            BindingOperations.GetBindingExpression((TextBox) sender, TextBox.TextProperty)?.UpdateSource();
            var datactx = (NHentaiViewModel) DataContext;
            datactx.HomeCommand.Execute(null);
        }
    }
}