using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for ExHentaiView.xaml
	/// </summary>
	public partial class ExHentaiView : UserControl {
		public static ExHentaiView Instance;

		public ExHentaiView() {
			Instance = this;
			InitializeComponent();
			DataContext = new ExHentaiViewModel();
		}

		private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e) {
			if (e.Key != Key.Enter) return;
			BindingOperations.GetBindingExpression((TextBox)sender, TextBox.TextProperty)?.UpdateSource();
			var datactx = (ExHentaiViewModel)DataContext;
			datactx.HomeCommand.Execute(null);
		}
	}
}