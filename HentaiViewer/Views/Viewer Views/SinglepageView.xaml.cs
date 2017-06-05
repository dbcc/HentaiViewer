using System.Windows.Controls;
using System.Windows.Input;

namespace HentaiViewer.Views.Viewer_Views {
    /// <summary>
    ///     Interaktionslogik für SinglepageView.xaml
    /// </summary>
    public partial class SinglepageView : UserControl {
        public SinglepageView() {
            InitializeComponent();
        }

        private void Flip_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.D:
                case Key.Space:
                    Flip.GoForward();
                    break;
                case Key.A:
                case Key.Back:
                    Flip.GoBack();
                    break;
            }
        }
    }
}