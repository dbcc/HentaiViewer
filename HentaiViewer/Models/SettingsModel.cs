using System.ComponentModel;
using Newtonsoft.Json;

namespace HentaiViewer.Models {
    public class SettingsModel {
        public ExhentaiOption ExHentai { get; set; } = new ExhentaiOption();

        public nHentaiOption nHentai { get; set; } = new nHentaiOption();

        public HentaiCafeOption Cafe { get; set; } = new HentaiCafeOption();

        public PururinOption Pururin { get; set; } = new PururinOption();

        public ApplicationOption Other { get; set; } = new ApplicationOption();
    }

    public class ApplicationOption {
        public bool ClickScroll { get; set; }

        public bool InstantFetch { get; set; }

        public ViewerSize ViewerSize { get; set; } = new ViewerSize();
    }

    public class ViewerSize {
        // Height="800" Width="900"

        public double Width { get; set; } = 800;

        public double Height { get; set; } = 800;
    }

    public class HentaiCafeOption {
        public string SearchQuery { get; set; } = string.Empty;
    }

    public class nHentaiOption {
        public string SearchQuery { get; set; } = string.Empty;
    }

    public class PururinOption {
        public string SearchQuery { get; set; } = string.Empty;
    }

    public class ExhentaiOption {
        public string IpbMemberId { get; set; } = string.Empty;

        public string IpbPassHash { get; set; } = string.Empty;

        public string Igneous { get; set; } = string.Empty;

        public string SearchQuery { get; set; } = string.Empty;


        public int Doujinshi { get; set; }


        public int Manga { get; set; }

        public int ArtistCg { get; set; }

        public int GameCg { get; set; }

        public int Western { get; set; }

        public int NonH { get; set; }

        public int ImageSet { get; set; }

        public int Cosplay { get; set; }

        public int AsianPorn { get; set; }

        public int Misc { get; set; }

        [DefaultValue(2), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinRating { get; set; }
    }
}