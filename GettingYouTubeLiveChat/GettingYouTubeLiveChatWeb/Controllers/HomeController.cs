using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApplication1.Models;

#region MITLicense
/*
Copyright 2020 naonao0001777

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITH
*/
#endregion

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
        [HttpPost]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            //if (aa=="aaa")
            //{
            //    System.Diagnostics.Process.Start(@"C:\temp");
            //}
            return View();
        }
    }
}