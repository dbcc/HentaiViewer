using System.Threading.Tasks;
using System.Windows;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;
using HentaiViewer.Views;

namespace HentaiViewer {
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private async void AppStartup(object sender, StartupEventArgs args) {
			HistoryController.Load();
			SettingsController.Load();
			FavoritesController.Load();
			await Task.Delay(200);
			var mainWindow = new MainWindow {
				DataContext = new MainWindowViewModel()
			};
			mainWindow.Show();
		}
	}
}