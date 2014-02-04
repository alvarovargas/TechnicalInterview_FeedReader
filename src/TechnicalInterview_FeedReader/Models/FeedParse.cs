using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace TechnicalInterview_FeedReader.Models
{
    public static class FeedParse
    {
        public static IEnumerable<FeedItem> GetFeedItems(Feed feed){
            try
            {
                XDocument feedXml = XDocument.Load(feed.Url);
                var items = from feeds in feedXml.Descendants("item")
                        select new FeedItem
                        {
                            Title = feeds.Element("title").Value,
                            ItemUrl = feeds.Element("link").Value,
                            //Text = Regex.Match(feeds.Element("description").Value, @"^.{1,180}\b(?<!\s)").Value
                            Text = feeds.Element("description").Value
                        };
                return items;
            }
            catch
            {

            }            
            return new List<FeedItem>();
        }

    }
}