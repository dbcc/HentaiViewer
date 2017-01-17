using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using HentaiViewer.Common;
using HentaiViewer.Models;
using RestSharp;

namespace HentaiViewer.Sites {
	public static class Pururin {
		public static async Task<List<HentaiModel>> GetLatestAsync(string url) {
			var document = await ParseHtmlStringAsync(await GetHtmlStringAsync(url));
			var hents = new List<HentaiModel>();
			var classes = document.All.Where(c => c.LocalName == "li" && c.ClassList.Contains("gallery-block"));
			//var a = classes.Where(x => x.OuterHtml == "img");
			var basepath = "http://pururin.us";
			foreach (var element in classes) {
				var img = await ParseHtmlStringAsync(element.InnerHtml);
				var title = img.All.First(t => t.LocalName == "h2").TextContent;
				hents.Add(new HentaiModel {
					Title = title,
					Link = basepath + img.Links[0].GetAttribute("href"),
					Img = new Uri(img.Images[0].Source.Replace("about://", basepath)),
					ThumbnailLink = img.Images[0].Source.Replace("about://", basepath),
					Site = "Pururin.us",
					Seen = HistoryController.CheckHistory(title, basepath + img.Links[0].GetAttribute("href"))
				});
			}
			return hents;
		}

		private static async Task<IHtmlDocument> ParseHtmlStringAsync(string html) {
			//We require a custom configuration
			var config = Configuration.Default.WithJavaScript();
			//Let's create a new parser using this configuration
			var parser = new HtmlParser(config);
			//Just get the DOM representation
			return await parser.ParseAsync(html);
		}

		private static async Task<string> GetHtmlStringAsync(string url) {
			var client = new RestClient {
				UserAgent =
					"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.70 Safari/537.36",
				Encoding = Encoding.UTF8,
				Timeout = 60000,
				BaseUrl = new Uri(url)
			};
			var request = new RestRequest();
			var response = await client.ExecuteGetTaskAsync(request);
			await Task.Delay(100);
			return response.Content;
		}

		public static async Task<Tuple<List<object>, int>> CollectImagesTaskAsync(HentaiModel hentai) {
			//http://pururin.us/gallery/32056/rem-ram-revolution
			//http://pururin.us/assets/image/data/32056/1.jpg
			var _galleryId = hentai.Link.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)[3];
			var document = await ParseHtmlStringAsync(await GetHtmlStringAsync(hentai.Link));
			var pages = int.Parse(Regex.Match(document.DocumentElement.OuterHtml, "<li>([0-9]+) Pages</li>").Groups[1].Value);
			if (hentai.Title == "lul") {
				var firstOrDefault = document.All.FirstOrDefault(h => h.LocalName == "h1" && h.ClassList.Contains("otitle"));
				if (firstOrDefault != null)
					hentai.Title = firstOrDefault.TextContent;
			}
			if (Directory.Exists(hentai.SavePath)) {
				//var files = Directory.GetFiles(hentai.SavePath, "*.png");
				var files =
					new DirectoryInfo(hentai.SavePath).GetFileSystemInfos("*.png").OrderBy(fs => int.Parse(fs.Name.Split('.')[0]));
				var paths = new List<object>();
				files.ToList().ForEach(p => paths.Add(p.FullName));
				if (files.ToList().Count == pages) return new Tuple<List<object>, int>(new List<object>(paths), pages);
			}
			var images = new List<object>();
			for (var i = 1; i <= pages; i++) images.Add($"http://pururin.us/assets/image/data/{_galleryId}/{i}.jpg");

			return new Tuple<List<object>, int>(images, pages);
		}
	}
}