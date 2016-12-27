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
	public class nHentai {
		public static async Task<List<HentaiModel>> GetLatest(string url) {
			var document = await ParseHtmlString(await GetHtmlString(url));
			var hents = new List<HentaiModel>();
			var classes = document.All.Where(c => c.LocalName == "a" && c.ClassList.Contains("cover"));
			//var a = classes.Where(x => x.OuterHtml == "img");
			foreach (var element in classes) {
				var img = await ParseHtmlString(element.InnerHtml);
				hents.Add(new HentaiModel {
					Title = img.All[0].TextContent,
					Link = $"https://nhentai.net{element.GetAttribute("href").Replace("about:", string.Empty)}",
					Img = new Uri($"https:{img.Images[0].Source.Replace("about:", string.Empty)}"),
					Site = "nhentai",
					Seen = HistoryController.CheckHistory(MD5Converter.MD5Hash(img.All[0].TextContent))
				});
			}
			return hents;
		}

		private static async Task<IHtmlDocument> ParseHtmlString(string html) {
			//Let's create a new parser using this configuration
			var parser = new HtmlParser(new Configuration().WithJavaScript());
			//Just get the DOM representation
			return await parser.ParseAsync(html);
		}

		private static async Task<string> GetHtmlString(string url) {
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
			var url = hentai.Link;
			var document = await ParseHtmlString(await GetHtmlString(url));
			var pages = int.Parse(Regex.Match(document.DocumentElement.OuterHtml, "<div>([0-9]+) pages</div>").Groups[1].Value);
			if (Directory.Exists(hentai.SavePath)) {
				var files = Directory.GetFiles(hentai.SavePath).Where(f => f.EndsWith(".png"));
				if (files.ToList().Count == pages) {

				}
				return new Tuple<List<object>, int>(new List<object>(Directory.GetFiles(hentai.SavePath).Where(f => f.EndsWith(".png"))), pages);
			}
			var imgTags = document.All.Where(i => i.LocalName == "img" && i.ClassList.Contains("lazyload"));

			var images = new List<object>();
			foreach (var element in imgTags)
				images.Add(
					$"https:{element.GetAttribute("data-src").Replace("t.j", ".j").Replace("/t.", "/i.").Replace("t.pn", ".pn")}");
			//img link https://t.nhentai.net/galleries/775571/6.jpg
			// overview link https://nhentai.net/g/124787/

			//var imagelinktmp = url.Replace("/g/", "/galleries/").Replace("://t.", "://i.");
			//for (int i = 1; i <= pages; i++) {
			//	images.Add($"{imagelinktmp}{i}.jpg");
			//}
			return new Tuple<List<object>, int>(images, pages);
		}
	}
}