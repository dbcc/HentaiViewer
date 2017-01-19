using System.Collections.ObjectModel;
using HentaiViewer.Common;
using HentaiViewer.Models;
using PropertyChanged;

namespace HentaiViewer.ViewModels {
    [ImplementPropertyChanged]
    public class HistoryViewModel {
        public HistoryViewModel() {
            HistoryItems = new ReadOnlyObservableCollection<HistoryModel>(HistoryController.History);
        }

        //private ObservableCollection<HistoryModel> _historyItems = new ObservableCollection<HistoryModel>(HistoryController.History);
        public ReadOnlyObservableCollection<HistoryModel> HistoryItems { get; }
    }
}