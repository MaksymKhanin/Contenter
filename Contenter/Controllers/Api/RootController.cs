using Contenter.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
                    Image = item?.album?.images[2].url.ToString(),
                    Duration = $"{time.Minutes}:{time.Seconds}"
                });
            }
            return list;
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

    }
}
