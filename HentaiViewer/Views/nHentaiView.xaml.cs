using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;

namespace HentaiViewer.Views {
	/// <summary>
	///     Interaction logic for nHentaiView.xaml
	/// </summary>
	public partial class nHentaiView : UserControl {
		public static nHentaiView Instance;

		public nHentaiView() {
			Instance = this;
			InitializeComponent();
			DataContext = new nHentaiViewModel();
		}
		
		private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e) {
			if (e.Key != Key.Enter) return;
			BindingOperations.GetBindingExpression((TextBox)sender, TextBox.TextProperty)?.UpdateSource();
			var datactx = (nHentaiViewModel)DataContext;
			datactx.HomeCommand.Execute(null);
		}
	}
}