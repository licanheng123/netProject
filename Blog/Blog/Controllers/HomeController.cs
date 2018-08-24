using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using Blog.Models;
using System.Linq.Expressions;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();

        public ActionResult Index()
        {
            var aticlesList = db.Articles
                .Where(p => p.IsPub == "是")
                .OrderByDescending(p => p.CreateTime)
                .Take(20).ToList();//如若这里不调用toList()，可能需要在前台访问的时候执行数据库

            return View(aticlesList);
        }

        public ActionResult PhotoAlbum(int page=1, int pageSize=8)
        {
            var query = db.Articles
                .Where(p => p.ShowType == "相册" && p.IsPub == "是")
                .OrderByDescending(p => p.CreateTime);

            var total = query.Count();
            var count = total / pageSize;
            //如果余数不是0
            if (total % pageSize != 0)
            {
                count++;
            }
            ViewBag.Count = count;

            var photosList = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            return View(photosList);
        }

        public ActionResult Article(ArticlesQuery articlesQuery)
        {
            var query = db.Articles
                .OrderByDescending(p => p.CreateTime)
                .Where(p => p.ShowType == "文章" && p.IsPub == "是");

            if (!string.IsNullOrEmpty(articlesQuery.title))
            {
                query = query.Where(p => p.Title.Contains(articlesQuery.title));
            }
            if (!string.IsNullOrEmpty(articlesQuery.type))
            {
                query = query.Where(p => p.Type == articlesQuery.type);
            }
            if (!string.IsNullOrEmpty(articlesQuery.tags))
            {
                query = query.Where(p => p.Tags == articlesQuery.tags);
            }
            var total = query.Count();
            var count = total / articlesQuery.limit;
            //如果余数是0
            if (total % articlesQuery.limit != 0)
            {
                count++;
            }
            var articleList = query
                .Skip((articlesQuery.page - 1) * articlesQuery.limit)
                .Take(articlesQuery.limit).ToList();

            ViewBag.Count = count;
            ViewBag.CurrentPage = articlesQuery.page;
            ViewBag.Search = articlesQuery.title;
            ViewBag.Type = articlesQuery.type;
            ViewBag.Tags = articlesQuery.tags;
            return View(articleList);
        }

        // 关于我
        // Get: /About/

        public ActionResult About()
        {
            var user = db.User.FirstOrDefault();
            return View(user);
        }

        public ActionResult LeaveMessage()
        {
            return View();
        }

        //
        // Get: /AticleDetail/

        public ActionResult ArticleDetail(int? id)
        {
            Articles articles = new Articles();
            if (id == null)
            {
                articles = db.Articles
                    .Where(p => p.ShowType=="文章")
                    .OrderByDescending(p => p.CreateTime)
                    .FirstOrDefault();
                //上一篇文章
                var perviousArticles = db.Articles
                    .Where(p => p.CreateTime < articles.CreateTime && p.ShowType == "文章")
                    .OrderByDescending(p => p.CreateTime).FirstOrDefault();
                ViewBag.PerviousArticles = perviousArticles;
                return View(articles);
            }
            else
            {
                articles = db.Articles.SingleOrDefault(p => p.Id == id);
                if (articles == null)
                {
                    return HttpNotFound();
                }
                //上一篇文章
                var perviousArticles = db.Articles
                    .Where(p => p.CreateTime < articles.CreateTime && p.ShowType == "文章")
                    .OrderByDescending(p => p.CreateTime).FirstOrDefault();
                //下一篇文章
                var nextArticles = db.Articles
                    .Where(p => p.CreateTime > articles.CreateTime && p.ShowType == "文章")
                    .OrderBy(p => p.CreateTime).FirstOrDefault();
                ViewBag.PerviousArticles = perviousArticles;
                ViewBag.NextArticles = nextArticles;
            }
            articles.ReadTimes += 1;
            db.SaveChanges();
            return View(articles);
        }

        //
        // Get: /PhotoAlbumDetail/

        public ActionResult PhotoAlbumDetail(int? id)
        {
            Articles articles = null;
            if (id == null)
            {
                articles = db.Articles
                    .Where(p => p.ShowType == "相册")
                    .OrderByDescending(p => p.CreateTime)
                    .FirstOrDefault();
            }
            else
            {
                articles = db.Articles.SingleOrDefault(p => p.Id == id);
                if (articles == null)
                {
                    return HttpNotFound();
                }
            }
            articles.ReadTimes += 1;
            db.SaveChanges();
            var photosList = db.Photos.Where(p => p.BelongToArticles == articles.Id).OrderByDescending(p => p.Id).ToList();
            ViewBag.PhotosList = photosList;
            ViewBag.PhotoLength = photosList.Count();
            return View(articles);
        }
    }
}
