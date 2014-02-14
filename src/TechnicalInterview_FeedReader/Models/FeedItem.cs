using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalInterview_FeedReader.Models
{
    public class FeedItem
    {
        public int ID { get; set; }
        public int FeedId { get; set; }
        public string ItemId { get; set; }
        public DateTime DatePublished { get; set; }
        public string Title { get; set; }
        public string ItemUrl { get; set; }
        public string Text { get; set; }
        public bool Read { get; set; }        
    }
}