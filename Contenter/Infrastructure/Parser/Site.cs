using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Contenter.Models;

namespace Contenter.Parser
{
    public class Site
    {
        public static SiteModel GetModelSite(string link)
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
            var titleTag=html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:title"));
            if (titleTag != null)
            {
                model.Title=titleTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
            }
            else
            {
                model.Title = html.DocumentNode.SelectNodes("//title").FirstOrDefault().InnerText;
            }

            var webSiteTag= html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:site_name"));
            if (webSiteTag != null)
            {
                model.WebSite= webSiteTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
            }

            var descriptionTag= html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:description"));
            if (descriptionTag != null)
            {
                model.Description=descriptionTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
            }
            else
            {
                descriptionTag= html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "name" && x.Value == "description"));
                if (descriptionTag != null)
                {
                    model.Description=descriptionTag.Attributes.FirstOrDefault(p => p.Name == "content").Value;
                }
            }

            var imageTag= html.DocumentNode.SelectNodes("//meta").FirstOrDefault(p => p.Attributes.Any(x => x.Name == "property" && x.Value == "og:image"));
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
        private static byte[] DonwloadImage(string link)
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