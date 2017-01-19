using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HentaiViewer.Models;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;

namespace HentaiViewer.Sites {
    public class Imgur {
        public static async Task<Tuple<List<object>, int>> CollectImagesTaskAsync(HentaiModel hentai) {
            var mockUrl = "https://api.imgur.com/3/album/5F5Cy/images";

            //https://imgur.com/a/LNKof
            var galleryId = hentai.Link.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Last();

            var client = new ImgurClient("b9a0d31f5189a6e");
            var endpoint = new AlbumEndpoint(client);
            IAlbum album;
            try {
                album = await endpoint.GetAlbumAsync(galleryId);
            }
            catch (ImgurException e) {
                hentai.Title = e.Message;
                return new Tuple<List<object>, int>(new List<object>(), 0);
            }
            if (hentai.Title == "lul") hentai.Title = album.Title ?? album.Link;
            if (Directory.Exists(hentai.SavePath)) {
                //var files = Directory.GetFiles(hentai.SavePath, "*.png");
                var files =
                    new DirectoryInfo(hentai.SavePath).GetFileSystemInfos("*.png")
                        .OrderBy(fs => int.Parse(fs.Name.Split('.')[0]));
                var paths = new List<object>();
                files.ToList().ForEach(p => paths.Add(p.FullName));
                if (files.ToList().Count == album.ImagesCount)
                    return new Tuple<List<object>, int>(new List<object>(paths), album.ImagesCount);
            }
            return new Tuple<List<object>, int>(new List<object>(album.Images.Select(i => i.Link).ToList()),
                album.ImagesCount);
        }
    }
}