using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using CloudFlareUtilities;
using HentaiViewer.Common;
using HentaiViewer.Models;
using RestSharp;

namespace HentaiViewer.Sites {
    public static class Pururin {
        public static async Task<List<HentaiModel>> GetLatestAsync(string url) {
            var document = await ParseHtmlStringAsync(await GetHtmlStringAsync(url));
            var hents = new List<HentaiModel>();
            var classes = document.All.Where(c => c.LocalName == "a" && c.ClassList.Contains("thumb-pururin"));
            //var a = classes.Where(x => x.OuterHtml == "img");

            foreach (var element in classes) {
                var img = element.Children.First(i => i.LocalName == "img").GetAttribute("src");
                hents.Add(new HentaiModel {
                    Title = element.TextContent.Trim('\n').Trim(),
                    Link = element.GetAttribute("href"),
                    Img = BytesToBitmapImage(await _httpClient.GetByteArrayAsync(img)),
                    ThumbnailLink = img,
                    Site = "Pururin.us",
                    Seen = HistoryController.CheckHistory(img)
                });
            }
            return hents;
        }

        public static BitmapImage BytesToBitmapImage(byte[] bytes) {
            if (bytes == null) return null;
            using (var mem = new MemoryStream(bytes, 0, bytes.Length)) {
                mem.Position = 0;
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
        private static async Task<IHtmlDocument> ParseHtmlStringAsync(string html) {
            //We require a custom configuration
            var config = Configuration.Default.WithJavaScript();
            //Let's create a new parser using this configuration
            var parser = new HtmlParser(config);
            //Just get the DOM representation
            return await parser.ParseAsync(html);
        }
        private static readonly ClearanceHandler ClearanceHandler = new ClearanceHandler{ MaxRetries = 2};
        private static HttpClient _httpClient;
        private static async Task<string> GetHtmlStringAsync(string url) {
            try {
                if (_httpClient == null) {
                    _httpClient = new HttpClient(ClearanceHandler);
                }
                var content = await _httpClient.GetStringAsync(url);
                await Task.Delay(100);
                return content;
            } catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException) {
                return null;
            } catch (AggregateException ex) when (ex.InnerException is TaskCanceledException) {
                return null;
            }

                    //var client = new RestClient {
                    //    UserAgent =
                    //        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.70 Safari/537.36",
                    //    Encoding = Encoding.UTF8,
                    //    Timeout = 60000,
                    //    BaseUrl = new Uri(url)
                    //};
                    //var request = new RestRequest();
                    //var response = await client.ExecuteGetTaskAsync(request);
                    //return response.Content;
        }

        public static async Task<Tuple<List<object>, int>> CollectImagesTaskAsync(HentaiModel hentai, Action<int, int> setPages) {
            if (Directory.Exists(hentai.SavePath) && hentai.IsSavedGallery) {
                var files =
                    new DirectoryInfo(hentai.SavePath).GetFileSystemInfos("*.???")
                        .OrderBy(fs => int.Parse(fs.Name.Split('.')[0]));
                var paths = new List<object>();
                files.ToList().ForEach(p => paths.Add(p.FullName));
                return new Tuple<List<object>, int>(new List<object>(paths), files.Count());
            }
            //http://pururin.us/gallery/32056/rem-ram-revolution
            //http://pururin.us/assets/image/data/32056/1.jpg
            var _galleryId = hentai.Link.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)[3];
            var document = await ParseHtmlStringAsync(await GetHtmlStringAsync(hentai.Link));
            var pages =
                int.Parse(Regex.Match(document.DocumentElement.OuterHtml, "<li>([0-9]+) Pages</li>").Groups[1].Value);
            if (hentai.Title == "lul") {
                var firstOrDefault =
                    document.All.FirstOrDefault(h => h.LocalName == "h1" && h.ClassList.Contains("otitle"));
                if (firstOrDefault != null)
                    hentai.Title = firstOrDefault.TextContent;
            }
            var images = new List<object>();
            for (var i = 1; i <= pages; i++) images.Add($"http://pururin.us/assets/image/data/{_galleryId}/{i}.jpg");

            return new Tuple<List<object>, int>(images, pages);
        }
    }
}