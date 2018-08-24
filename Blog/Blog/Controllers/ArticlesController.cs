using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using Blog.Models;

namespace Blog.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();

        //
        // GET: /ArticlesIndex/

        public ActionResult ArticlesIndex()
        {
            ViewBag.TypeList = GetTypeList();
            return View();
        }

        // 提供获取文章列表的数据接口
        // Get: /ArticlesList/

        public string ArticlesList(ArticlesQuery articlesQuery) 
        {
            var query = db.Articles.Where(p => p.ShowType=="文章");
            if (!String.IsNullOrEmpty(articlesQuery.title))
            {
                query = query.Where(p => p.Title.Contains(articlesQuery.title));
            }

            if (!String.IsNullOrEmpty(articlesQuery.type))
            {
                query = query.Where(p => p.Type == articlesQuery.type);
            }
            var articlesList = query.OrderByDescending(u => u.CreateTime)
                .Skip((articlesQuery.page - 1) * articlesQuery.limit)
                .Take(articlesQuery.limit).ToList();
            int count = articlesList.Count();
            StringBuilder strJson = new StringBuilder();
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            foreach (var item in articlesList)
            {
                strJson.Append(JsonConvert.SerializeObject(item, iso) + ",");
            }
            return "{\"code\":0,\"msg\":\"\",\"count\":" + count + ",\"data\":[" + strJson.ToString().TrimEnd(',') + "]}";
        }

        //
        // GET: /ArticlesAdd/

        public ActionResult ArticlesAdd()
        {
            return View();
        }

        // 
        // Post: /ArticlesAdd/
        [HttpPost]
        public ActionResult ArticlesAdd(Articles articles) 
        {
            articles.ReadTimes = 0;
            articles.CreateTime = DateTime.Now;
            if(articles.IsPub == "on")
            {
                articles.IsPub = "是";
                articles.PubTime = DateTime.Now;
            }else{
                articles.IsPub = "否";
            }
            articles.ShowType = "文章";
            db.Articles.Add(articles);
            db.SaveChanges();
            return RedirectToAction("ArticlesIndex");
        }

        //
        // GET: /ArticlesEdit/

        public ActionResult ArticlesEdit(int id)
        {
            var articles = db.Articles
                .Where(p => p.Id == id)
                .SingleOrDefault();
            return View(articles);
        }

        // 
        // Post: /Articles/
        [HttpPost]
        public void ArticlesEdit(Articles articles)
        {
            var articlesObj = db.Articles.SingleOrDefault(p => p.Id == articles.Id);
            if (articles.IsPub == "是")
            {
                if (articlesObj.IsPub == "否")
                {
                    articlesObj.IsPub = "是";
                    articlesObj.PubTime = DateTime.Now;
                }
            }
            else
            {
                articlesObj.IsPub = "否";
            }
            articlesObj.Title = articles.Title;
            articlesObj.Tags = articles.Tags;
            articlesObj.SimpleInfo = articles.SimpleInfo;
            articlesObj.Cont = articles.Cont;
            articlesObj.MainPicUrl = articles.MainPicUrl;
            articlesObj.Source = articles.Source;
            articlesObj.Type = articles.Type;
            db.SaveChanges();
        }

        // 文章删除的接口
        // Post: /ArticlesDelete/
        [HttpPost]
        public ActionResult ArticlesDelete(int id)
        {
            var articls = new Articles {Id = id };
            db.Articles.Attach(articls);
            db.Articles.Remove(articls);
            db.SaveChanges();
            return Json("删除成功");
        }

        // 文章发布的接口
        // Post: /ArticlesPub/

        [HttpPost]
        public  ActionResult ArticlesPub(int id, string flag)
        {
            var articles = db.Articles.SingleOrDefault(p => p.Id == id);
            var message = "";
            if ("true".Equals(flag))
            {
                articles.IsPub = "是";
                articles.PubTime = DateTime.Now;
                message = "发布成功";
            }
            else
            {
                articles.IsPub = "否";
                message = "发布取消成功";
            }
            db.SaveChanges();
            return Json(message);
        }

        // 多文章发布接口
        // Post: /ArticlesPubMultiple/

        [HttpPost]
        public ActionResult ArticlesPubMultiple(string ids) 
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Json("请选择要发布的对象");
            }
            string[] idstrArr = ids.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] idArr = Array.ConvertAll<string, int>(idstrArr, delegate(string s) { return int.Parse(s); });
            var q = from p in db.Articles
                    where idArr.Contains(p.Id)
                    select p;
            foreach (var p in q)
            {
                p.IsPub = "是";
                p.PubTime = DateTime.Now;
            }
            db.SaveChanges();
            /*string[] idstrArr = ids.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] idArr = Array.ConvertAll<string, int>(idstrArr, delegate(string s) { return int.Parse(s); });
            var articlesList = unitOfWork.ArticlesRepository.Get(fliter: p => idArr.Contains(p.Id));
            foreach (var item in articlesList)
            {
                if (item.IsPub != "是")
                {
                    item.IsPub = "是";
                    unitOfWork.ArticlesRepository.Update(item);
                }
            }
            unitOfWork.Save();
            unitOfWork.Dispose();*/
            return Json("发布成功");
        }

        // 获取展示的文章类型集合

        private List<SelectListItem> GetTypeList()
        {
            var typeList = db.Articles.Where(p => p.ShowType == "文章").Select(p => p.Type).Distinct().ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in typeList)
	        {
		       list.Add(new SelectListItem { Text=item,Value=item });
	        }
            return list;
        }
    }
}
