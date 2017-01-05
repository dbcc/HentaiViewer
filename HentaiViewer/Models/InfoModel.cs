using System;

namespace HentaiViewer.Models {
	public class InfoModel {
		public InfoModel(HentaiModel hentai = null, int imgcount = 0) {
			if (hentai != null) {
				Link = hentai.Link;
				ImageCount = imgcount;
				Title = hentai.Title;
				Site = hentai.Site;
				SavedTime = DateTime.Now;
				Thumbnail = hentai.ThumbnailLink;
			}
		}

		public string Link { get; set; }
		public int ImageCount { get; set; }
		public string Title { get; set; }
		public string Site { get; set; }
		public DateTime SavedTime { get; set; }
		public object Thumbnail { get; set; }
	}
}