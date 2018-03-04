using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebProxy.Areas.MyWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.baidu.com");
            var response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            var encode = string.Empty;
            if (response.CharacterSet == "ISO-8859-1")
                encode = "GB2312";
              else
                encode = response.CharacterSet;
            var sr = new StreamReader(stream, Encoding.GetEncoding(encode));
            var html = sr.ReadToEnd();
            ViewData["msg"] = html;
            return View();
        }
    }
}
