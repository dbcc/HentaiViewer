using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace HentaiViewer.Common {
    internal static class GithubController {
        public static readonly float _tag = 0.88f;
        public static string GithubUrl { get; private set; }

        public static async Task<bool> CheckForUpdateAsync() {
            try {
                var client = new RestClient {
                    UserAgent =
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.70 Safari/537.36",
                    Encoding = Encoding.UTF8,
                    Timeout = 60000,
                    BaseUrl = new Uri("https://api.github.com/repos/tensei/HentaiViewer/releases")
                };
                var request = new RestRequest();
                var response = await client.ExecuteGetTaskAsync(request);
                var jsonSettings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};
                var api = JsonConvert.DeserializeObject<List<GithubApi>>(response.Content, jsonSettings);
                var tag = float.Parse(api.First().tag_name, CultureInfo.InvariantCulture);
                GithubUrl = api.First().html_url;
                return _tag < tag;
            }
            catch {
                return false;
            }
        }
    }

    //public class Author {
    //	public string login { get; set; }
    //	public int id { get; set; }
    //	public string avatar_url { get; set; }
    //	public string gravatar_id { get; set; }
    //	public string url { get; set; }
    //	public string html_url { get; set; }
    //	public string followers_url { get; set; }
    //	public string following_url { get; set; }
    //	public string gists_url { get; set; }
    //	public string starred_url { get; set; }
    //	public string subscriptions_url { get; set; }
    //	public string organizations_url { get; set; }
    //	public string repos_url { get; set; }
    //	public string events_url { get; set; }
    //	public string received_events_url { get; set; }
    //	public string type { get; set; }
    //	public bool site_admin { get; set; }
    //}

    //public class Uploader {
    //	public string login { get; set; }
    //	public int id { get; set; }
    //	public string avatar_url { get; set; }
    //	public string gravatar_id { get; set; }
    //	public string url { get; set; }
    //	public string html_url { get; set; }
    //	public string followers_url { get; set; }
    //	public string following_url { get; set; }
    //	public string gists_url { get; set; }
    //	public string starred_url { get; set; }
    //	public string subscriptions_url { get; set; }
    //	public string organizations_url { get; set; }
    //	public string repos_url { get; set; }
    //	public string events_url { get; set; }
    //	public string received_events_url { get; set; }
    //	public string type { get; set; }
    //	public bool site_admin { get; set; }
    //}

    //public class Asset {
    //	public string url { get; set; }
    //	public int id { get; set; }
    //	public string name { get; set; }
    //	public object label { get; set; }
    //	public Uploader uploader { get; set; }
    //	public string content_type { get; set; }
    //	public string state { get; set; }
    //	public int size { get; set; }
    //	public int download_count { get; set; }
    //	public string created_at { get; set; }
    //	public string updated_at { get; set; }
    //	public string browser_download_url { get; set; }
    //}

    public class GithubApi {
        //public string url { get; set; }
        //public string assets_url { get; set; }
        //public string upload_url { get; set; }
        public string html_url { get; set; }

        //public int id { get; set; }
        public string tag_name { get; set; }
        //public string name { get; set; }

        //public string target_commitish { get; set; }
        //public bool draft { get; set; }
        //public Author author { get; set; }
        //public bool prerelease { get; set; }
        //public string created_at { get; set; }
        //public string published_at { get; set; }
        //public List<Asset> assets { get; set; }
        //public string tarball_url { get; set; }
        //public string zipball_url { get; set; }
        //public string body { get; set; }
    }
}