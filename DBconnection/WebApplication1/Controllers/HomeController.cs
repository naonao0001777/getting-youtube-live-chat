using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApplication1.Models;

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
        public async Task<ActionResult> About(string param)
        {
            
            YoutubeAPI youtube = new Models.YoutubeAPI();


            LiveChatModelList asd = await youtube.IndexYoutube(param);

            ViewData["PostData"] = "動画ID" + param + "を検索しました。";// + "結果：\t" + comprehensiveText;
            ViewBag.Model = asd;
            return View(asd);
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