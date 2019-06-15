using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }

        // ホーム画面から値を取得
        [HttpPost]
        public ActionResult About(string param)
        {
            ViewData["PostData"] = param + "を検索しました。";
            Models.YoutubeAPI youtube = new Models.YoutubeAPI();
            Task t = youtube.IndexYoutube(param);

            return View();
        }

        /// <summary>
        /// サンプル画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// サンプル画面2
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// サンプル画面3
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}