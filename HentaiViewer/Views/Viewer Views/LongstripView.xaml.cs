using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views.Viewer_Views
{
    /// <summary>
    /// Interaktionslogik für LongstripView.xaml
    /// </summary>
    public partial class LongstripView : UserControl
    {
        public LongstripView()
        {
            InitializeComponent();
            //loop timer
            _loopTimer = new Timer
            {
                Interval = 10,
                Enabled = false,
                AutoReset = true
            };
            // interval in milliseconds
            _loopTimer.Elapsed += LoopTimerEvent;
        }
        private static Timer _loopTimer;

        private int _direction;
        
        private async void LoopTimerEvent(object source, ElapsedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () => {
                var x = scviewer.VerticalOffset;
                scviewer.ScrollToVerticalOffset(x + _direction);
                x = scviewer.VerticalOffset;
                //todo load images
                if (!(scviewer.VerticalOffset + scviewer.ViewportHeight >= scviewer.ExtentHeight - 50)) return;
                var data = (HentaiViewerWindowViewModel)DataContext;
                if (data.ImageObjects.Count > 50) scviewer.ScrollToTop();
                await data.LoadMoreImagesAsync();
            }));
        }
        
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _loopTimer.Enabled = true;
                _direction = (int)SliderScrollSpeed.Value;
            }
            if (e.ChangedButton != MouseButton.Right) return;
            _loopTimer.Enabled = true;
            _direction = -(int)SliderScrollSpeed.Value;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _loopTimer.Enabled = false;
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            _loopTimer.Enabled = false;
        }
        
        private async void Scviewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!(scviewer.VerticalOffset + scviewer.ViewportHeight >= scviewer.ExtentHeight - 100)
                || Math.Abs(scviewer.VerticalOffset) <= 0
                || CanvasMouseover.Visibility == Visibility) return;

            var data = (HentaiViewerWindowViewModel)DataContext;
            if (data.ImageObjects.Count > 50) scviewer.ScrollToTop();
            await data.LoadMoreImagesAsync();
        }
    }
}
