using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
    /// <summary>
    ///     Interaction logic for HentaiViewerWindow.xaml
    /// </summary>
    public partial class HentaiViewerWindow {
        public HentaiViewerWindow() {
            InitializeComponent();
        }
        

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
        
        
        private void HentaiViewerWindow_OnClosed(object sender, EventArgs e) {
            var data = (HentaiViewerWindowViewModel) DataContext;
            data.Dispose();
            GC.Collect();
            DataContext = null;
            SettingsController.Save();
        }
    }
}