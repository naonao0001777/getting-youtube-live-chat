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
        /// 検索画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// 結果画面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Result(string param)
        {
            bool errFlg = false;

            YoutubeAPI youtube = new YoutubeAPI();

            LiveChatModelList chatModelList = await youtube.IndexYoutube(param);

            if (chatModelList.ChatList == null)
            {
                errFlg = true;
            }

            ViewData["PostData"] = "動画ID：" + param + "を検索しました。";// + "結果：\t" + comprehensiveText;

            ViewBag.errFlg = errFlg;

            return View(chatModelList);
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
        /// コメント検索結果画面
        /// </summary>
        /// <returns></returns>
        public ActionResult CommentResult(string param)
        {
            ViewBag.Message = "コメント検索結果";
            LiveChatModelList modelList = new LiveChatModelList();

            List<LiveChatModel> resultList = new List<LiveChatModel>();
            
            foreach (LiveChatModel model in modelList.ChatList)
            {
                if (model.DspMessage.Contains(param))
                {
                    resultList.Add(model);
                }
                
            }
            return View(resultList);
        }

        /// <summary>
        /// このサイトについて画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}