using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Text;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebProxy.Areas.MyWeb.Controllers
{
  
    public class HomeController : Controller
    {
        // GET: /<controller>/ 
        [HttpPost,HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Request.Method != "POST")
                return View();
            string urlStr = HttpContext.Request.Form["url"];
            string refStr = HttpContext.Request.Form["ref"];
            string cookieStr = HttpContext.Request.Form["cookie"];

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlStr);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";

            if(!string.IsNullOrEmpty(refStr))
                request.Referer = refStr;

            CookieContainer myCookieContainer = new CookieContainer();
            if(!string.IsNullOrEmpty(cookieStr))
                myCookieContainer.SetCookies(new Uri(urlStr), cookieStr);
            request.CookieContainer = myCookieContainer;

            request.Timeout = 30*1000;
            try
            {
                var reqResponse = (HttpWebResponse)request.GetResponse();
                var stream = reqResponse.GetResponseStream();
 
                var sr = new StreamReader(stream);

                var ccoding = reqResponse.Headers["content-Encoding"];
                var clength = reqResponse.Headers["content-length"];
                var ctype = reqResponse.Headers["content-type"];
                var ccookie = reqResponse.Headers["cookie"];
                if (ccoding != null)
                    Response.Headers.Add("content-Encoding", ccoding);
                if (clength != null)
                    Response.Headers.Add("content-length", clength);
                if (ctype != null)
                    Response.Headers.Add("content-type", ctype);
                if (ccookie != null)
                    Response.Headers.Add("cookie", ccookie);

               byte[] buf = new byte[1024];
               int len = stream.Read(buf, 0, buf.Length);
               while (len > 0)
               {
                   Response.Body.Write(buf, 0, len);
                   len = stream.Read(buf, 0, buf.Length);
               }
               Response.Body.Flush();
                Response.Body.Close();
            }
            catch(Exception ex) {
                ViewData["msg"] = "出错了。"+ex.Message;
            }
            return View();
        }
    }
}
