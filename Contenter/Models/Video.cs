﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Contenter.Models
{
    public class Video
    {
        public int Id { get; set; }
      
        public string Link { get; set; }
    }
}