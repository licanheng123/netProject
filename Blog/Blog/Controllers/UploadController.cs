using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        //
        // Post: /Upload/

        /// <summary>
        /// 实现上传图片的action
        /// </summary>
        [HttpPost]
        public string UploadImage()
        {
            //定义错误消息
            string message = "",src="";
            int code = 0;
            try
            {
                HttpPostedFileBase postFile = Request.Files["file"];
                if (postFile != null)
                {
                    string fileExt = postFile.FileName.Substring(postFile.FileName.LastIndexOf(".") + 1); //文件扩展名，不含“.”
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + fileExt; //随机生成新的文件名
                    string upLoadPath = "/upload/images/"; //上传目录相对路径
                    string fullUpLoadPath = Server.MapPath(upLoadPath); //上传目录的物理路径
                    string newFilePath = upLoadPath + newFileName; //上传后的路径
                    postFile.SaveAs(fullUpLoadPath + newFileName);
                    src = newFilePath;
                }
                else
                {
                    message = "请选择要上传文件！";
                    code = 1;
                }
            }
            catch (Exception)
            {
                message = "上传失败";
                code = 1;
            }
            return "{\"code\":" + code + ",\"msg\":\"" + message + "\",\"data\":{\"src\":\"" + src + "\"}}";
        }

        //
        // Post: /Upload/

        /// <summary>
        /// 实现多张图片上传的action
        /// </summary>
        [HttpPost]
        public string UploadImageMultiple()
        { 
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //定义错误消息
            string message = "",src="";
            int code = 0;
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    if (file.FileName == "" || file.ContentLength == 0)
                    {
                        code = 1;
                        message = "上传的文件不允许为空";
                        break;
                    }
                    else
                    {
                        string fileExt = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1); //文件扩展名，不含“.”
                        string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + fileExt; //随机生成新的文件名
                        string upLoadPath = "/upload/images/"; //上传目录相对路径
                        string fullUpLoadPath = Server.MapPath(upLoadPath); //上传目录的物理路径
                        string newFilePath = upLoadPath + newFileName; //上传后的路径
                        file.SaveAs(fullUpLoadPath + newFileName);
                        src += newFilePath + ",";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                code = 1;
            }
            return "{\"code\":" + code + ",\"msg\":\"" + message + "\",\"data\":{\"src\":\"" + src + "\"}}";
        }
    }
}
