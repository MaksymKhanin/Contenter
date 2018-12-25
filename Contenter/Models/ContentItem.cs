using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contenter.Models
{
    public class ContentItem
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string LinkId { get; set; }
        public string DateTimeAdded { get; set; }
    }
}