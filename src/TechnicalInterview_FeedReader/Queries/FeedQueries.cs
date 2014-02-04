using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TechnicalInterview_FeedReader.Models;

namespace TechnicalInterview_FeedReader.Queries
{
    public static class FeedQueries
    {
        public static Feed FindById(this IQueryable<Feed> feeds, int id)
        {
            return feeds.Single(r => r.ID == id);
        }

        public static IEnumerable<Feed> ListByUser(DbSet<Feed> feeds, string userName)
        {
            return feeds.Where(r => r.UserName == userName);
        }

        internal static IEnumerable<FeedItem> SearchAll(DbSet<Feed> feeds, string userName, string searchStr)
        {
            List<FeedItem> list = new List<FeedItem>();
            searchStr = searchStr.Trim();
            try
            {
                if (searchStr == "" || searchStr == "*")
                {
                    foreach (Feed feed in FeedQueries.ListByUser(feeds, userName))
                    {
                        list.AddRange(FeedParse.GetFeedItems(feed));
                    }
                }
                else
                {
                    // TODO: find a better way to do this... 
                    foreach (Feed feed in FeedQueries.ListByUser(feeds, userName))
                    {
                        foreach (FeedItem item in FeedParse.GetFeedItems(feed))
                        {
                            foreach (string term in searchStr.Split())
                            {
                                if (item.Title.ToUpper().Contains(term.ToUpper()) ||
                                    (new Regex("<(.|\\n)+?>")).Replace(item.Text, " ").ToUpper().Contains(term.ToUpper()))
                                {
                                    list.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                list = new List<FeedItem>();
                // ignore the exception...
                // TODO: Log exception to log function
            }            
            return list;
        }
    }
}