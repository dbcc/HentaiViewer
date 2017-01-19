using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.ViewModels;
using HentaiViewer.Views;
using Newtonsoft.Json;

namespace HentaiViewer.Models {
	public class HistoryModel {
		[JsonEncrypt]
		public string Title { get; set; }
		[JsonEncrypt]
		public string Link { get; set; }
		[JsonEncrypt]
		public string Site { get; set; }
		public DateTime Date { get; set; }
		[JsonIgnore]
		public string DaysOld => DaysSinceUpdate();

		[JsonIgnore]
		public ICommand ViewCommand { get; }
		[JsonIgnore]
		public ICommand DeleteCommand { get; }

		public HistoryModel() {
			ViewCommand = new ActionCommand(View);
			DeleteCommand = new ActionCommand(() => {
				HistoryController.History.Remove(this);
				HistoryController.Save();
			});
		}

		private void View()
		{
			//if (Site == "exhentai") return;
			var viewWindow = new HentaiViewerWindow {
				DataContext = new HentaiViewerWindowViewModel(new HentaiModel {
					Title = Title,
					Link = Link,
					Site = Site
				}),
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};
			viewWindow.Show();
		}

		private string DaysSinceUpdate() {
			var dateNow = DateTime.Now;
			var diff = dateNow - Date;
			if (diff.Days < 0) return "Unknown";
			return diff.Days == 0 ? "Today" : $"{diff.Days} day(s) ago";
		}
	}
}