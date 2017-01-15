using System.ComponentModel;

namespace HentaiViewer.Models {
	public class SettingsModel {
		public ExhentaiOption ExHentai { get; set; }
		public nHentaiOption nHentai { get; set; }
		public HentaiCafeOption Cafe { get; set; }
		public PururinOption Pururin { get; set; }

	}

	public class HentaiCafeOption {
		[DefaultValue("")]
		public string SearchQuery { get; set; }
	}

	public class nHentaiOption {
		[DefaultValue("")]
		public string SearchQuery { get; set; }
	}
	public class PururinOption {
		[DefaultValue("")]
		public string SearchQuery { get; set; }
	}

	public class ExhentaiOption {
		[DefaultValue("")]
		public string IpbMemberId { get; set; }

		[DefaultValue("")]
		public string IpbPassHash { get; set; }

		[DefaultValue("")]
		public string Igneous { get; set; }

		[DefaultValue("")]
		public string SearchQuery { get; set; }

		[DefaultValue(0)]
		public int Doujinshi { get; set; }

		[DefaultValue(0)]
		public int Manga { get; set; }

		[DefaultValue(0)]
		public int ArtistCg { get; set; }

		[DefaultValue(0)]
		public int GameCg { get; set; }

		[DefaultValue(0)]
		public int Western { get; set; }

		[DefaultValue(0)]
		public int NonH { get; set; }

		[DefaultValue(0)]
		public int ImageSet { get; set; }

		[DefaultValue(0)]
		public int Cosplay { get; set; }

		[DefaultValue(0)]
		public int AsianPorn { get; set; }

		[DefaultValue(0)]
		public int Misc { get; set; }
	}
}