using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HentaiViewer.Models;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;

namespace HentaiViewer.Sites {
	public class Imgur {
		public static async Task<Tuple<List<object>, int>> CollectImagesTaskAsync(HentaiModel hentai) {
			var mockUrl = "https://api.imgur.com/3/album/5F5Cy/images";

			//https://imgur.com/a/LNKof
			var galleryId = hentai.Link.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Last();

			var client = new ImgurClient("b9a0d31f5189a6e");
			var endpoint = new AlbumEndpoint(client);
			var images = await endpoint.GetAlbumImagesAsync(galleryId);

			if (hentai.Title == "lul") {
				var firstOrDefault = images.FirstOrDefault();
				if (firstOrDefault != null) hentai.Title = firstOrDefault.Description?.Replace("\n", " ") ?? hentai.Link;
			}
			if (Directory.Exists(hentai.SavePath)) {
				//var files = Directory.GetFiles(hentai.SavePath, "*.png");
				var files =
					new DirectoryInfo(hentai.SavePath).GetFileSystemInfos("*.png").OrderBy(fs => int.Parse(fs.Name.Split('.')[0]));
				var paths = new List<object>();
				files.ToList().ForEach(p => paths.Add(p.FullName));
				if (files.ToList().Count == images.Count())
					return new Tuple<List<object>, int>(new List<object>(paths), images.Count());
			}
			return new Tuple<List<object>, int>(new List<object>(images.Select(i => i.Link).ToList()), images.Count());
		}
	}
}