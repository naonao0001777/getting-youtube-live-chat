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
        public async Task<ActionResult> Result(string param, string quantity, string count="0")
        {
            bool errFlg = false;

            // YouTubeAPI共通メソッド
            YoutubeAPI youtube = new YoutubeAPI();

            // APIサービス基本メソッド
            LiveChatModelList chatModelList = await youtube.IndexYoutube(param,quantity);

            if (chatModelList.ChatList == null)
            {
                errFlg = true;
            }

            ViewData["PostData"] = param;
            ViewData["PostQuantity"] = quantity;

            ViewBag.errFlg = errFlg;
            ViewBag.count = count;
            return View(chatModelList);
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
            ViewBag.Message = "このサイトの制作者";

            return View();
        }
    }
}