﻿using System;
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
        public async Task<ActionResult> Result(string param, string quantity, string count = "0")
        {
            bool errFlg = false;

            string service = "Live";

            // YouTubeAPI共通メソッド
            YoutubeAPI youtube = new YoutubeAPI();

            // APIサービス基本メソッド
            LiveChatModelList chatModelList = await youtube.IndexYoutube(param, quantity, service);

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
        /// コメント検索画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            // 初期表示
            string display_mode = "INITIAL";

            ViewData["DISPLAY_MODE"] = display_mode;

            return View();
        }

        /// <summary>
        /// コメント検索結果（POST）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Search(string param)
        {
            // 再表示
            string display_mode = "POST";

            if (string.IsNullOrEmpty(param))
            {
                display_mode = "INITIAL";
            }

            ViewData["DISPLAY_MODE"] = display_mode;
            ViewData["SEARCH_ID"] = param;

            // サービス識別子
            string service = "Search";

            // YoutubeAPI共通
            YoutubeAPI youtubeAPI = new YoutubeAPI();

            // APIサービス基本メソッド
            LiveChatModelList commentModelList = await youtubeAPI.IndexYoutube(param, null, service);

            return View(commentModelList);
        }

        /// <summary>
        /// CSVダウンロード
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CsvDownload(string csvbutton, string param, HttpResponseBase response)
        {
            // 再表示
            string display_mode = "POST";

            if (string.IsNullOrEmpty(param))
            {
                display_mode = "INITIAL";
            }

            ViewData["DISPLAY_MODE"] = display_mode;
            ViewData["SEARCH_ID"] = param;

            // サービス識別子
            string service = "Search";

            // YoutubeAPI共通
            YoutubeAPI youtubeAPI = new YoutubeAPI();

            // APIサービス基本メソッド
            LiveChatModelList commentModelList = await youtubeAPI.IndexYoutube(param, null, service);

            // CSV出力
            youtubeAPI.CsvDownloader(param, commentModelList, response);

            return View("Search", commentModelList);
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