using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Contenter.Models
{
    public class Audio
    {
        [Key]
        public int AudioId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Duration { get; set; }
    }
}