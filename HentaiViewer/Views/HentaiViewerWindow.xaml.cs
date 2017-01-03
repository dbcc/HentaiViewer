using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for HentaiViewerWindow.xaml
	/// </summary>
	public partial class HentaiViewerWindow {
		private static Timer _loopTimer;

		private int _direction;

		public HentaiViewerWindow() {
			InitializeComponent();
			//loop timer
			_loopTimer = new Timer {
				Interval = 10,
				Enabled = false
			};
			// interval in milliseconds
			_loopTimer.Elapsed += LoopTimerEvent;
			_loopTimer.AutoReset = true;
		}
		
		private void LoopTimerEvent(object source, ElapsedEventArgs e) {
			Application.Current.Dispatcher.BeginInvoke(new Action(() => {
				var x = scviewer.VerticalOffset;
				scviewer.ScrollToVerticalOffset(x + _direction);
				x = scviewer.VerticalOffset;
				//todo load images
				//if (scviewer.VerticalOffset + scviewer.ViewportHeight >= scviewer.ExtentHeight -100) {
				//	Debug.Print("At the bottom of the list!");
				//}
			}));
		}

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left) DragMove();
		}


		private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left) {
				_loopTimer.Enabled = true;
				_direction = (int) SliderScrollSpeed.Value;
			}
			if (e.ChangedButton != MouseButton.Right) return;
			_loopTimer.Enabled = true;
			_direction = -(int) SliderScrollSpeed.Value;
		}

		private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
			_loopTimer.Enabled = false;
		}

		private void Canvas_MouseLeave(object sender, MouseEventArgs e) {
			_loopTimer.Enabled = false;
		}

		private void HentaiViewerWindow_OnClosed(object sender, EventArgs e) {
			var data = (HentaiViewerWindowViewModel) DataContext;
			data.Dispose();
			GC.Collect();
			DataContext = null;
		}

		private void Scviewer_OnScrollChanged(object sender, ScrollChangedEventArgs e) {


		}
	}
}