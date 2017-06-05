using PropertyChanged;

namespace HentaiViewer.Models {
    [ImplementPropertyChanged]
    public class ImageModel {
        public int PageNumber { get; set; }
        public object Source { get; set; }
        public bool IsGif { get; set; }
    }
}