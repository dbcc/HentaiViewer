using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HentaiViewer.Models {
	public class SettingsModel {
		public ExhentaiOption ExHentai { get; set; }
		public nHentaiOptions nHentai { get; set; }
		public HentaiCafe Cafe { get; set; }
	}

	public class HentaiCafe {
		
	}

	public class nHentaiOptions {

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
