using System.Windows;

namespace HentaiViewer.Models {
    public class DrawerListBoxItem {
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool Separator { get; set; }
        public bool IsEnabled { get; set; } = true;
        public FrameworkElement View { get; set; }
    }
}