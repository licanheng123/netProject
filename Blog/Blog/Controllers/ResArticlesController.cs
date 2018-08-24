using Blog.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    [Authorize]
    public class RecArticlesController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();
        //
        // GET: /RecArticlesIndex/

        public ActionResult RecArticlesIndex()
        {
            return View();
        }

        // 获取推荐列表的数据接口
        // Get: /RecArticlesList/
        public string RecArticlesList(int page, int limit)
        {
            var query = db.RecArticlesViews;
            int count = query.Count();
            var recArticlesViewList = query.OrderByDescending(u => u.RecId).Skip((page - 1) * limit).Take(limit).ToList();
            return "{\"code\":0,\"msg\":\"\",\"count\":" + count + ",\"data\":" + JsonConvert.SerializeObject(recArticlesViewList) + "}";
        }       

        //
        // GET: /RecArticlesAdd/

        public ActionResult RecArticlesAdd(int? id)
        {
            var articles = db.Articles.SingleOrDefault(p => p.Id == id);
            var maxWeight = db.RecArticles.Max(p => p.Weight);
            ViewBag.Weight = maxWeight + 1;
            return View(articles);
        }

        //
        // POST: /RecArticlesAdd/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecArticlesAdd(RecArticles recArticles)
        {
            db.RecArticles.Add(recArticles);
            db.SaveChanges();
            return Json("添加推荐成功");
        }

        //
        // GET: /RecArticlesEdit/

        public ActionResult RecArticlesEdit(int id)
        {
            var recArticles = db.RecArticles.SingleOrDefault(p => p.Id == id);
            var articles = db.Articles.SingleOrDefault(p => p.Id == recArticles.ArticleId);
            ViewBag.Articles = articles;
            return View(recArticles);
        }

        //
        // Post: /RecArticlesEdit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void RecArticlesEdit(RecArticles recArticles)
        {
            db.Entry(recArticles).State = EntityState.Modified;
            db.SaveChanges();
        }

        //
        // Post: /RecArticlesDelete/
        [HttpPost]
        public ActionResult RecArticlesDelete(int id)
        {
            RecArticles recArticles = new RecArticles {Id = id };
            db.RecArticles.Attach(recArticles);
            db.RecArticles.Remove(recArticles);
            db.SaveChanges();
            return Json("删除成功");
        }
    }
}
