using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contenter.Models
{
    public class SiteModel
    {
        public string Title { get; set; }
        public string WebSite { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string Url { get; set; }
    }
}