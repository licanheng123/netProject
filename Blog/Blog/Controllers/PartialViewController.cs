using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class PartialViewController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();

        //
        // GET: /AboutMe/

        public ActionResult AboutMe()
        {
            User user = HttpContext.Cache["Self"] as User;
            if (user == null)
            {
                user = db.User.FirstOrDefault();
                HttpContext.Cache.Insert("Self",user,null, DateTime.Now.AddMinutes(5),System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return View(user);
        }

        //
        // GET: /Photos/

        public ActionResult Photos(int? posId)
        {
            IEnumerable<RecArticlesView> recArticlesViewList = null;
            if (posId == null)
            {
                recArticlesViewList = db.RecArticlesViews.Where(p => p.ShowType == "相册").OrderByDescending(p => p.Weight).ToList();
            }
            else
            {
                recArticlesViewList = db.RecArticlesViews.Where(p => p.ShowType == "相册" && p.PosId == posId).OrderByDescending(p => p.Weight).ToList();
            }
            return View(recArticlesViewList);
        }

        //
        // GET: /Commend/

        public ActionResult Commend(int? posId)
        {
            IEnumerable<RecArticlesView> recArticlesViewList = null;
            if (posId == null)
            {
                recArticlesViewList = db.RecArticlesViews.Where(p => p.ShowType == "文章").OrderByDescending(p => p.Weight).ToList();
            }
            else
            {
                recArticlesViewList = db.RecArticlesViews.Where(p => p.ShowType == "文章" && p.PosId == posId).OrderByDescending(p => p.Weight).ToList();
            }
            return View(recArticlesViewList);
        }

        //
        // GET: /ArticlesType/

        public ActionResult ArticlesType()
        {
            var group = db.Articles
                .Where(p => p.ShowType == "文章" && p.IsPub == "是")
                .GroupBy(p => p.Type).ToList();

            return View(group);
        }

        //
        // GET: /TagsCloud/

        public ActionResult TagsCloud()
        {
            var tagsList = db.Articles
                .Where(p => p.ShowType == "文章")
                .Select(s => s.Tags)
                .Distinct()
                .ToList();
            return View(tagsList);
        }

    }
}
