using Contenter.Models;
using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;


namespace Contenter.Controllers.Api
{
    public class RootController : ApiController
    {


        public ResponseMessageResult MakeResponseError(int statusCode, string message)
        {
            return new ResponseMessageResult(Request.CreateErrorResponse((HttpStatusCode)statusCode, new HttpError(message)));
        }


        public ResponseMessageResult MakeCustomResponse(int statusCode, object message)
        {
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)statusCode, message));
        }

        public async Task<List<SpotifyTrack>> GetSearchTrack(string track)
        {
            var list = new List<SpotifyTrack>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.spotify.com/v1/search?q=" + track + "&type=track&limit=10");//
            request.Method = "GET";
            request.KeepAlive = true;
            request.ContentType = "appication/json";
            request.Headers.Add("Authorization", $"Bearer {(await GetToken()).AccessToken}");
            //request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }
            dynamic obj = JObject.Parse(myResponse);
            foreach (var item in obj.tracks.items)
            {
                TimeSpan time = TimeSpan.FromMilliseconds(Convert.ToDouble(item?.duration_ms.ToString()));
                list.Add(new SpotifyTrack
                {
                    Name = item?.artists[0]?.name.ToString() + " - " + item?.name.ToString(),
                    Id = item?.id.ToString(),
                    Image = item?.album?.images[1].url.ToString(),
                    Duration = $"{time.Minutes}:{time.Seconds}"
                });
            }
            return list;
        }
        public async Task<SpotifyTrack> GetTrackById(string id)
        {
            var track = new SpotifyTrack();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.spotify.com/v1/tracks/" + id);
            request.Method = "GET";
            request.KeepAlive = true;
            request.ContentType = "appication/json";
            request.Headers.Add("Authorization", $"Bearer {(await GetToken()).AccessToken}");
            //request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }
            dynamic obj = JObject.Parse(myResponse);
            TimeSpan time = TimeSpan.FromMilliseconds(Convert.ToDouble(obj?.duration_ms.ToString()));
            var second = time.Seconds < 10 ? "0" : "" + $"{ time.Seconds}";
            track = new SpotifyTrack
            {
                Name = obj?.artists[0]?.name.ToString() + " - " + obj?.name.ToString(),
                Id = obj?.id.ToString(),
                Image = obj?.album?.images[1].url.ToString(),
                Duration = $"{time.Minutes}:{second}"
            };

            return track;
        }

        public async Task<SpotifyToken> GetToken()
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
               { "grant_type", "client_credentials" },
               { "client_id", "a7a2631664dd4bbdb85a401a92f94cc9" },
               { "client_secret", "07f92c911ab741fda3326f5a6db38b2d" }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<SpotifyToken>(responseString);
            return token;
        }

        public SiteModel GetModelSite(string link)
        {
            string htmlCode;
            SiteModel model = new SiteModel();
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = client.DownloadString(link);
            }
            var html = new HtmlDocument();
            html.LoadHtml(htmlCode);
            var titleTag = html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:title"));
            if (titleTag != null)
            {
                model.Title = titleTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
            }
            else
            {
                model.Title = html.DocumentNode.SelectNodes("//title").FirstOrDefault().InnerText;
            }

            var webSiteTag = html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:site_name"));
            if (webSiteTag != null)
            {
                model.WebSite = webSiteTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
            }

            var descriptionTag = html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:description"));
            if (descriptionTag != null)
            {
                model.Description = descriptionTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
            }
            else
            {
                descriptionTag = html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "name" && x.Value == "description"));
                if (descriptionTag != null)
                {
                    model.Description = descriptionTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
                }
            }

            var imageTag = html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:image"));
            if (imageTag != null)
            {
                model.Image = DonwloadImage(imageTag.Attributes.FirstOrDefault(p => p.Name == "content").Value);
            }
            else
            {
                imageTag = html.DocumentNode.SelectNodes("//link").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "rel" && x.Value == "shortcut icon"));
                if (imageTag != null)
                {
                    model.Image = DonwloadImage(imageTag.Attributes.FirstOrDefault(p => p.Name == "href").Value);
                }
            }
            model.Url = link;
            return model;

        }
        private byte[] DonwloadImage(string link)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] pic = client.DownloadData(link);
                    return pic;
                }
            }
            catch
            {
                return null;
            }
        }



    }
}
