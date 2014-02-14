using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechnicalInterview_FeedReader.Models
{
    public class Feed
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Url]
        [Required]
        public string Url { get; set; }
        public string UserName { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}