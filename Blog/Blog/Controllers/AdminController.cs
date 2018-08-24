using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Blog.Models;

namespace Blog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();
        //
        // GET: /Admin/Login

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Admin/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password, string memory)
        {
            bool isMemory = Request.Form["memory"] == null ? false : true;
            string returnUrl = Request.Form["ReturnUrl"];
            //验证数据
            var user = db.User.FirstOrDefault(u => u.UserName == userName && (u.Password == password));
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(userName, isMemory);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    user.LastLoginTime = DateTime.Now;
                    db.SaveChanges();
                    return Redirect("~/Admin/Message");
                }
            }
            else
            {
                ViewBag.LoginState = "用户名或密码错误，请重试";
            }
            return View();
        }

        //
        // Get: /Admin/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }
        // 
        // GET: /Admin/Message

        public ActionResult Message()
        {
            User user = db.User.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            return View(user);
        }
        //
        // Post: /Admin/Message
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Message(User user)
        {
            User user1 = db.User.FirstOrDefault(u => u.UserName == user.UserName);
            user1.SelfPhoto = user.SelfPhoto;
            user1.SelfSlogo = user.SelfSlogo;
            user1.AboutMe = user.AboutMe;
            db.SaveChanges();
            ViewBag.WarnMsg = "基本资料更新成功";
            return View(user1);
        }
        //
        // GET: /Admin/Password
        public ActionResult Password()
        {
            return View();
        }
        //
        // Post: /Admin/Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Password(string oldPassword, string newPassword)
        {
            User user = db.User.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user.Password != oldPassword)
            {
                ViewBag.WarnMsg = "旧密码错误";
            }
            else
            {
                user.Password = newPassword;
                db.SaveChanges();
                ViewBag.WarnMsg = "修改成功";
            }
            return View();
        }
    }
}
