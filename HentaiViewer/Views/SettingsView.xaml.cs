using System;
using System.Windows;
using System.Windows.Controls;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
    /// <summary>
    ///     Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl {
        public SettingsView() {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }

        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e) {
            var media = (MediaElement) sender;
            media.Position = new TimeSpan(0, 0, 1);
            media.Play();
        }
    }
}