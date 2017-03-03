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
        public static async Task<List<HentaiModel>> GetLatestAsync(string url) {
            var document = await ParseHtmlStringAsync(await GetHtmlStringAsync(url));
            var hents = new List<HentaiModel>();
            var classes = document.All.Where(c => c.LocalName == "a" && c.ClassList.Contains("cover"));
            //var a = classes.Where(x => x.OuterHtml == "img");
            foreach (var element in classes) {
                var img = await ParseHtmlStringAsync(element.InnerHtml);
                var i =
                    img.All.FirstOrDefault(
                        ii =>
                            ii.LocalName == "img" && ii.HasAttribute("data-src") &&
                            ii.GetAttribute("data-src").StartsWith("https://t.nhentai.net/galleries/"))?.GetAttribute("data-src") ??
                    $"https:{img.Images[0].Source.Replace("about:", string.Empty)}";
                var title = img.All.FirstOrDefault(t => t.LocalName== "div" && t.ClassList.Contains("caption"))?.TextContent;
                try {
                    hents.Add(new HentaiModel {
                        Title = title,
                        Link = $"https://nhentai.net{element.GetAttribute("href").Replace("about:", string.Empty)}",
                        Img = i,
                        ThumbnailLink = i,
                        Site = "nHentai.net",
                        Seen =
                            HistoryController.CheckHistory(img.All[0].TextContent,
                                $"https://nhentai.net{element.GetAttribute("href").Replace("about:", string.Empty)}")
                    });
                }
                catch (Exception ) {
                    // hmm
                }
            }
            return hents;
        }

        private static async Task<IHtmlDocument> ParseHtmlStringAsync(string html) {
            //Let's create a new parser using this configuration
            var parser = new HtmlParser(new Configuration().WithJavaScript());
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
            if (Directory.Exists(hentai.SavePath) && hentai.IsSavedGallery) {
                var files =
                    new DirectoryInfo(hentai.SavePath).GetFileSystemInfos("*.png")
                        .OrderBy(fs => int.Parse(fs.Name.Split('.')[0]));
                var paths = new List<object>();
                files.ToList().ForEach(p => paths.Add(p.FullName));
                return new Tuple<List<object>, int>(new List<object>(paths), files.Count());
            }
            var url = hentai.Link;
            var document = await ParseHtmlStringAsync(await GetHtmlStringAsync(url));
            var pages =
                int.Parse(Regex.Match(document.DocumentElement.OuterHtml, "<div>([0-9]+) pages</div>").Groups[1].Value);
            if (hentai.Title == "lul") {
                var firstOrDefault = document.All.FirstOrDefault(h => h.LocalName == "h1");
                if (firstOrDefault != null)
                    hentai.Title = firstOrDefault.TextContent;
            }
            var imgTags = document.All.Where(i => i.LocalName == "img" && i.ClassList.Contains("lazyload"));

            var images =
                imgTags.Select(
                        element =>
                            $"{element.GetAttribute("data-src").Replace("t.j", ".j").Replace("/t.", "/i.").Replace("t.pn", ".pn")}")
                    .Cast<object>()
                    .ToList();
            //img link https://t.nhentai.net/galleries/775571/6.jpg
            // overview link https://nhentai.net/g/124787/

            //var imagelinktmp = url.Replace("/g/", "/galleries/").Replace("://t.", "://i.");
            //for (int i = 1; i <= pages; i++) {
            //	images.Add($"{imagelinktmp}{i}.jpg");
            //}

            var ims = new List<object>();
            foreach (var image in images) {
                if (image.ToString().StartsWith("http")) {
                    ims.Add(image);
                    continue;
                }
                ims.Add($"https:{image}");
            }
            return new Tuple<List<object>, int>(ims, pages);
        }
    }
}