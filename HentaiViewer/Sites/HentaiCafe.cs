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
	public class HentaiCafe {
		public static async Task<List<HentaiModel>> GetLatest(string url) {
			var document = await ParseHtmlString(await GetHtmlString(url));
			var hents = new List<HentaiModel>();
			var classes = document.All.Where(c => c.LocalName == "a" && c.ClassList.Contains("entry-thumb"));
			//var a = classes.Where(x => x.OuterHtml == "img");
			foreach (var element in classes) {
				var img = await ParseHtmlString(element.InnerHtml);
				var title = element.GetAttribute("title").Split(new[] {'"'}, StringSplitOptions.RemoveEmptyEntries)[1];
				hents.Add(new HentaiModel {
					Title = title,
					Link = element.GetAttribute("href"),
					Img = new Uri(img.Images[0].Source),
					Site = "hentaicafe",
					Seen = HistoryController.CheckHistory(MD5Converter.MD5Hash(title))
				});
			}
			return hents;
		}

		private static async Task<IHtmlDocument> ParseHtmlString(string html) {
			//We require a custom configuration
			var config = Configuration.Default.WithJavaScript();
			//Let's create a new parser using this configuration
			var parser = new HtmlParser(config);
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
			var entryPage = await ParseHtmlString(await GetHtmlString(url));
			var entryLink =
				entryPage.All.Where(l => l.LocalName == "a" && l.ClassList.Contains("x-btn-large")).ToList()[0].GetAttribute("href");

			if (!entryLink.EndsWith("page/1"))
				entryLink = entryLink + "page/1";
			var html = await GetHtmlString(entryLink);
			var match = Regex.Match(html,
				"<div class=\"text\">([0-9]+) ⤵</div>",
				RegexOptions.IgnoreCase);
			var retlist = new List<object>();
			var lastChapterNumber = int.Parse(match.Groups[1].Value);
			if (Directory.Exists(hentai.SavePath)) {
				var files = Directory.GetFiles(hentai.SavePath).Where(f => f.EndsWith(".png"));
				if (files.ToList().Count == lastChapterNumber) {

				}
				return new Tuple<List<object>, int>(new List<object>(Directory.GetFiles(hentai.SavePath).Where(f => f.EndsWith(".png"))), lastChapterNumber);
			}
			var slitlink = entryLink.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
			slitlink[slitlink.Length - 1] = "1";
			var newlink = string.Join("/", slitlink);
			newlink = newlink.Replace(":/", "://");

			var htmlimg = await GetHtmlString(newlink);

			var imgLink = Regex.Match(htmlimg,
				@"([https|http]+://[a-z]+\.?[a-z]+?\.[a-z]+.+/content/comics/.+[\.jpg|\.png])");
			retlist.Add(imgLink.Groups[1].Value);

			for (var i = 2; i <= lastChapterNumber; i++) {
				var img = imgLink.Groups[1].Value.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
				var suffixsplit = img.Last().Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
				var extension = suffixsplit.Last();
				//var imgnumber = int.Parse(suffixsplit[0]);
				var newsuffix = i < 10 ? $"0{i}.{extension}" : $"{i}.{extension}";
				img[img.Length - 1] = newsuffix;
				var nimg = string.Join("/", img).Replace(":/", "://");
				retlist.Add(nimg);
			}
			return new Tuple<List<object>, int>(retlist, lastChapterNumber);
		}
	}
}