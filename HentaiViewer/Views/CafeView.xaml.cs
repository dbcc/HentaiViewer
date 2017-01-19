using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
    /// <summary>
    ///     Interaction logic for CafeView.xaml
    /// </summary>
    public partial class CafeView : UserControl {
        public static CafeView Instance;

        public CafeView() {
            Instance = this;
            InitializeComponent();
            DataContext = new HentaiCafeViewModel();
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) return;
            BindingOperations.GetBindingExpression((TextBox) sender, TextBox.TextProperty)?.UpdateSource();
            var datactx = (HentaiCafeViewModel) DataContext;
            datactx.HomeCommand.Execute(null);
        }
    }
}