using QDFeedParser;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace TechnicalInterview_FeedReader.Models
{
    public class FeedMachine
    {
        //private static FeedsDB db = new FeedsDB();
        public static void CreateNewFeed(Feed newFeed)
        {
            using (FeedsDB db = new FeedsDB())
            {
                if (db.Feeds.Where(r => r.Url == newFeed.Url && r.UserName == newFeed.UserName).Count() > 0)
                {
                    // already exists
                    // TODO: give user error message
                }
                else
                {

                    Uri feeduri = new Uri(newFeed.Url);
                    IFeedFactory factory = new HttpFeedFactory();
                    IFeed feed = factory.CreateFeed(feeduri);

                    if (feed != null)
                    {
                        newFeed.LastUpdated = feed.LastUpdated;
                        db.Feeds.Add(newFeed);
                        db.SaveChanges(); // save.. to get id

                        /* IFeed members...
                        FeedType FeedType
                        string FeedUri
                        string Generator
                        List<BaseFeedItem> Items
                        DateTime LastUpdated
                        string Link
                        string Title
                        */

                        foreach (BaseFeedItem feedItem in feed.Items)
                        {
                            /* IFeedItem members...
                            string Author
                            IList<string> Categories
                            string Content
                            DateTime DatePublished
                            string Id
                            string Link
                            string Title
                             */
                            if (db.FeedItems.Where(r => r.FeedId == newFeed.ID && r.ItemId == feedItem.Id).Count() == 0)
                            {
                                FeedItem newItem = new FeedItem();
                                newItem.Read = false;
                                newItem.FeedId = newFeed.ID;
                                newItem.ItemId = feedItem.Id;
                                newItem.Text = feedItem.Content;
                                newItem.Title = feedItem.Title;
                                newItem.ItemUrl = feedItem.Link;
                                newItem.DatePublished = feedItem.DatePublished;
                                db.FeedItems.Add(newItem);
                            }
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        // bad url???
                        // TODO: give user error message                    
                    }
                }
            }

        }

        public static void UpdateAll(string userName)
        {
            List<Feed> list;
            using (FeedsDB db = new FeedsDB())
            {
                list = db.Feeds.Where(r => r.UserName == userName).ToList();            
            }            
            foreach (Feed feed in list)
            {
                UpdateFeed(feed);
            }
        }

        private static void UpdateFeed(Feed feed)
        {
            //LastUpdated
            Uri feeduri = new Uri(feed.Url);
            IFeedFactory factory = new HttpFeedFactory();
            IFeed ifeed = factory.CreateFeed(feeduri);
            if (feed.LastUpdated != ifeed.LastUpdated)
            {
                feed.LastUpdated = ifeed.LastUpdated;
                using (FeedsDB db = new FeedsDB())
                {
                    foreach (BaseFeedItem feedItem in ifeed.Items)
                    {
                        /* IFeedItem members...
                        string Author
                        IList<string> Categories
                        string Content
                        DateTime DatePublished
                        string Id
                        string Link
                        string Title
                         */
                        if (db.FeedItems.Where(r => r.FeedId == feed.ID && r.ItemId == feedItem.Id).Count() == 0)
                        {
                            FeedItem newItem = new FeedItem();
                            newItem.Read = false;
                            newItem.FeedId = feed.ID;
                            newItem.ItemId = feedItem.Id;
                            newItem.Text = feedItem.Content;
                            newItem.Title = feedItem.Title;
                            newItem.ItemUrl = feedItem.Link;
                            newItem.DatePublished = feedItem.DatePublished;
                            db.FeedItems.Add(newItem);
                        }
                    }
                    db.SaveChanges();
                }
            }
        }

        public static Feed FindFeedById(int feedId)
        {
            using (FeedsDB db = new FeedsDB())
            {
                return (Feed)db.Feeds.Single(r => r.ID == feedId);
            }
        }
        public static void DeleteFeedById(int feedId)
        {
            using (FeedsDB db = new FeedsDB())
            {
                var feedItemsToDelete =
                        from fi in db.FeedItems
                        where fi.FeedId == feedId
                        select fi;

                foreach (var fi in feedItemsToDelete)
                {
                    db.FeedItems.Remove(fi);
                }

                var feedToDelete =
                        from f in db.Feeds
                        where f.ID == feedId
                        select f;
                foreach (var feed in feedToDelete)
                {
                    db.Feeds.Remove(feed);
                }                
                db.SaveChanges();                
            }

        }

        public static void MarkItemAsRead(int feedItemId)
        {
            using (FeedsDB db = new FeedsDB())
            {
                var feedItemsToUpdate =
                        from fi in db.FeedItems
                        where fi.ID == feedItemId
                        select fi;

                foreach (var fi in feedItemsToUpdate)
                {
                    fi.Read = true;
                }
                db.SaveChanges();
            }
        }

        public static IEnumerable<Feed> ListFeedsByUser(string userName)
        {
            using (FeedsDB db = new FeedsDB())
            {
                return db.Feeds.Where(r => r.UserName == userName).ToList();
            }
        }

        public static IEnumerable<FeedItem> SearchAll(string userName, string searchStr)
        {
            using (FeedsDB db = new FeedsDB())
            {
                // prepare searchStr
                searchStr =  (searchStr == null || searchStr == "*") ? "" : searchStr;
                string[] searchstrings = searchStr.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                searchstrings = searchstrings.Select(x => x.ToUpper()).ToArray();

                var final = (from f in db.Feeds
                             join fi in db.FeedItems on f.ID equals fi.FeedId
                             where f.UserName == userName && fi.Read == false
                             orderby fi.DatePublished descending
                             select fi).ToList()
                             .Where(x => searchstrings.All(y => (x.Title.ToUpper() + " " + (new Regex("<(.|\\n)+?>")).Replace(x.Text, " ").ToUpper()).Contains(y)));

                return final.ToList();
            }
            
        }

    }

}