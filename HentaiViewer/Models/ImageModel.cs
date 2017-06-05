using System.ComponentModel;

namespace HentaiViewer.Models {
    public class ImageModel : INotifyPropertyChanged {
        public int PageNumber { get; set; }
        public object Source { get; set; }
        public bool IsGif { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}