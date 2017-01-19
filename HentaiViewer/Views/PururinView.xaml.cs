using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) return;
            BindingOperations.GetBindingExpression((TextBox) sender, TextBox.TextProperty)?.UpdateSource();
            var datactx = (PururinViewModel) DataContext;
            datactx.HomeCommand.Execute(null);
        }
    }
}