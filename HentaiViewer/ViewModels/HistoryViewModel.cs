using System.Collections.ObjectModel;
using System.ComponentModel;
using HentaiViewer.Common;
using HentaiViewer.Models;

namespace HentaiViewer.ViewModels {
    public class HistoryViewModel : INotifyPropertyChanged {
        public HistoryViewModel() {
            HistoryItems = new ReadOnlyObservableCollection<HistoryModel>(HistoryController.History);
        }

        //private ObservableCollection<HistoryModel> _historyItems = new ObservableCollection<HistoryModel>(HistoryController.History);
        public ReadOnlyObservableCollection<HistoryModel> HistoryItems { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}