using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnicalInterview_FeedReader.Models;

namespace TechnicalInterview_FeedReader.Controllers
{

    [Authorize]
    public class FeedController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Search");
        }

        public ActionResult Manage()
        {
            return View(FeedMachine.ListFeedsByUser(User.Identity.Name));
        }

        public ActionResult UpdateAll(string returnUrl)
        {
            FeedMachine.UpdateAll(User.Identity.Name);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }            
        }
                
        public ActionResult Search(string searchString = "*")
        {
            return View(FeedMachine.SearchAll(User.Identity.Name, searchString));
        }

        public ActionResult Create()
        {
            return View(new Feed());
        }

        [HttpPost]
        public ActionResult Create(Feed newFeed)
        {
            newFeed.UserName = User.Identity.Name;
            FeedMachine.CreateNewFeed(newFeed);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Read(int feedItemId = 0)
        {
            if (feedItemId > 0) { FeedMachine.MarkItemAsRead(feedItemId); }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = -1)
        {
            if (id == -1)
            {
                return RedirectToAction("Index");
            }
            return View(FeedMachine.FindFeedById(id));
        }

        [HttpPost]
        public ActionResult Delete(Feed feed)
        {
            FeedMachine.DeleteFeedById(feed.ID);
            return RedirectToAction("Manage");
        }

        

    }
}
