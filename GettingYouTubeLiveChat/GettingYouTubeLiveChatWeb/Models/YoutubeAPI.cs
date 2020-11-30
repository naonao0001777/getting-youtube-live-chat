using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;
using System.Threading.Tasks;
#region MITLicense
/*
Copyright 2020 naonao0001777

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITH
*/
#endregion
namespace WebApplication1.Models
{
    public class YoutubeAPI
    {
        /// <summary>
        ///  メッセージ
        /// </summary>
        public string ChatMessage { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string ChatName { get; set; }

        /// <summary>
        /// メッセージリスト
        /// </summary>
        public List<string> MessageList { get; set; }
        
        /// <summary>
        /// 名前リスト
        /// </summary>
        public List<string> NameList { get; set; }


        public async Task<LiveChatModelList> IndexYoutube(string param)
        {
            LiveChatModelList testa = new LiveChatModelList();
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyARlEuRMw7DIRcgpuC-18TRTHqcwCxTaNc"
                });

                // 動画IDを入力
                string liveChatId = GetLiveChatID(param, youtubeService);

                testa = await GetLiveChatMessage(liveChatId, youtubeService, null);
            }
            catch (Exception)
            {
            }

            return testa;
        }

        /// <summary>
        /// 動画IDを取得する
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="youTubeService"></param>
        /// <returns></returns>
        static public string GetLiveChatID(string videoId, YouTubeService youTubeService)
        {
            var videosList = youTubeService.Videos.List("LiveStreamingDetails");

            videosList.Id = videoId;

            var videoListResponse = videosList.Execute();

            foreach (var videoID in videoListResponse.Items)
            {
                return videoID.LiveStreamingDetails.ActiveLiveChatId;
            }
            // 動画情報が取得できなかった場合はNULLを返す
            return null;
        }

        /// <summary>
        /// 動画コメントを取得する
        /// </summary>
        /// <param name="liveChatId"></param>
        /// <param name="youtubeService"></param>
        /// <param name="nextPageToken"></param>
        /// <returns></returns>
        static public async Task<LiveChatModelList> GetLiveChatMessage(string liveChatId, YouTubeService youtubeService, string nextPageToken)
        {

            List<string> liveMessagesList = new List<string>();

            var liveChatRequest = youtubeService.LiveChatMessages.List(liveChatId, "snippet,authorDetails");
            liveChatRequest.PageToken = nextPageToken;

            Google.Apis.YouTube.v3.Data.LiveChatMessageListResponse liveChatResponse = liveChatRequest.Execute();


            LiveChatModelList list = new LiveChatModelList();

            List<LiveChatModel> instantList = new List<LiveChatModel>();

            // ループ処理をして文字列型リストに格納
            foreach (var liveChat in liveChatResponse.Items)
            {
                //Console.WriteLine($"{liveChat.Snippet.DisplayMessage},{liveChat.AuthorDetails.DisplayName}");
                try
                {
                    LiveChatModel lcInfo = new LiveChatModel();
                    lcInfo.DspMessage = liveChat.Snippet.DisplayMessage;
                    lcInfo.DspName = liveChat.AuthorDetails.DisplayName;

                    instantList.Add(lcInfo);

                }
                catch (Exception ext)
                {
                    // 例外処理
                    string errMessage = ext.Message;
                    Console.Write(errMessage);
                }
            }
             list.ChatList = instantList;
             await Task.Delay((int)liveChatResponse.PollingIntervalMillis);

            return list;
           
        }
        
    }
    //await GetLiveChatMessage(liveChatId, youtubeService, liveChatResponse.NextPageToken);
    //return liveMessagesList;
}