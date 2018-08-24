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
    public class PhotosController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();

        //
        // GET: /PhotosIndex/

        public ActionResult PhotosIndex()
        {
            return View();
        }

        // 提供获取相册数据列表的数据接口
        // Post: /PhotosList/

        public string PhotosList(int page, int limit)
        {
            var query = db.Articles.Where(p => p.ShowType == "相册");
            StringBuilder strJson = new StringBuilder();
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            int count = query.Count();
            var articlesList = query.OrderByDescending(p => p.CreateTime).Skip((page - 1) * limit).Take(limit).ToList();
            foreach (var item in articlesList)
            {
                strJson.Append(JsonConvert.SerializeObject(item, iso) + ",");
            }
            return "{\"code\":0,\"msg\":\"\",\"count\":" + count + ",\"data\":[" + strJson.ToString().TrimEnd(',') + "]}";
        }

        //
        // GET: /PhotosAdd/

        public ActionResult PhotosAdd()
        {
            return View();
        }

        //
        // Post: /PhotosAdd/

        [HttpPost]
        public ActionResult PhotosAdd(Articles articles)
        {
            articles.ReadTimes = 0;
            articles.CreateTime = DateTime.Now;
            if (articles.IsPub == "on")
            {
                articles.IsPub = "是";
                articles.PubTime = DateTime.Now;
            }
            else
            {
                articles.IsPub = "否";
            }
            articles.ShowType = "相册";
            db.Articles.Add(articles);
            db.SaveChanges();
            return RedirectToAction("PhotosIndex");
        }

        //
        // GET: /PhotosEdit/

        public ActionResult PhotosEdit(int id)
        {
            var articles = db.Articles.SingleOrDefault(p => p.Id == id);
            return View(articles);
        }

        //
        // Post: /PhotosEdit/
        [HttpPost]
        public void PhotosEdit(Articles articles)
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

        //
        // GET: /PhotosDelete/

        public ActionResult PhotosDelete(int id)
        {
            Articles articls = new Articles { Id = id };
            db.Articles.Attach(articls);
            db.Articles.Remove(articls);
            db.SaveChanges();
            return Json("删除成功");
        }

        //
        // GET: /PhotosManage/

        public ActionResult PhotosManage(int id)
        {
            var articles = db.Articles.FirstOrDefault(p => p.Id == id);
            var photosList = db.Photos.Where(p => p.BelongToArticles == id).ToList();
            ViewBag.ArticlesId = articles.Id;
            ViewBag.ArticlesTitle = articles.Title;
            return View(photosList);
        }

        //
        // GET: /PhotosUpload/

        public ActionResult PhotosManageUpload(int id)
        {
            ViewBag.ArticlesId = id;
            return View();
        }

        //
        // Post: /PhotosUpload/
        [HttpPost]
        public ActionResult PhotosManageUpload(Photos photos)
        {
            db.Photos.Add(photos);
            db.SaveChanges();
            return RedirectToAction("PhotosManage", new { id = photos.BelongToArticles });
        }

        //
        // GET: /PhotosManageEdit/

        public ActionResult PhotosManageEdit(int id)
        {
            var photos = db.Photos.SingleOrDefault(p => p.Id == id);
            return View(photos);
        }

        //
        // Post: /PhotosManageEdit/
        [HttpPost]
        public ActionResult PhotosManageEdit(Photos photos)
        {
            db.Entry(photos).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PhotosManage", new { id = photos.BelongToArticles });
        }

        //
        // Post: /PhotosManageDelete/
        [HttpPost]
        public ActionResult PhotosManageDelete(int id)
        {
            Photos photos = new Photos { Id = id };
            db.Photos.Attach(photos);
            db.Photos.Remove(photos);
            db.SaveChanges();
            return Json("删除成功");
        }

        //
        // Post: /PhotosPub/

        [HttpPost]
        public ActionResult PhotosPub(int id, string flag)
        {
            var articles = db.Articles.SingleOrDefault(p => p.Id == id);
            var message = "";
            if ("true".Equals(flag))
            {
                articles.IsPub = "是";
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

        //
        // Post: /PhotosPubMultiple/

        [HttpPost]
        public ActionResult PhotosPubMultiple(string ids)
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
    }
}
