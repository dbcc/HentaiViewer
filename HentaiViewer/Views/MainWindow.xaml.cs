namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		public static MainWindow Instance;

		public MainWindow() {
			InitializeComponent();
			Instance = this;
		}
	}
}