using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnicalInterview_FeedReader.Models;
using TechnicalInterview_FeedReader.Queries;

namespace TechnicalInterview_FeedReader.Controllers
{

    [Authorize]
    public class FeedController : Controller
    {

        FeedsDB db = new FeedsDB();

        public ActionResult Index()
        {
            return RedirectToAction("Search");
        }

        public ActionResult Manage()
        {
            return View(FeedQueries.ListByUser(db.Feeds, User.Identity.Name));
        }
                
        public ActionResult Search(string searchString = "*")
        {
            return View(FeedQueries.SearchAll(db.Feeds, User.Identity.Name, searchString));
        }

        public ActionResult Create()
        {
            return View(new Feed());
        }

        [HttpPost]
        public ActionResult Create(Feed newFeed)
        {
            newFeed.UserName = User.Identity.Name;
            db.Feeds.Add(newFeed);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = -1)
        {
            if (id == -1)
            {
                return RedirectToAction("Index");
            }
            var feed = FeedQueries.FindById(db.Feeds, id);
            return View(feed);
        }

        [HttpPost]        
        public ActionResult Edit(Feed feed)
        {
            var feedToEdit = FeedQueries.FindById(db.Feeds, feed.ID);
            feedToEdit.Name = feed.Name;
            feedToEdit.Url = feed.Url;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = -1)
        {
            if (id == -1)
            {
                return RedirectToAction("Index");
            }
            var feed = FeedQueries.FindById(db.Feeds, id);
            return View(feed);
        }

        [HttpPost]
        public ActionResult Delete(Feed feed)
        {
            db.Feeds.Remove(FeedQueries.FindById(db.Feeds, feed.ID));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        public ActionResult Details(int id = -1)
        {
            if (id == -1)
            {
                return RedirectToAction("Index");
            }
            var feed = FeedQueries.FindById(db.Feeds, id);
            ViewBag.CurrentFeedTitle = feed.Name;
            return View(FeedParse.GetFeedItems(FeedQueries.FindById(db.Feeds, id)));
        }

    }
}
