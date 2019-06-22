using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class YoutubeAPI
    {
        public string ChatMessage { get; set; }

        public string ChatName { get; set; }

        public List<string> MessageList { get; set; }

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
            string liveMsg = null;
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