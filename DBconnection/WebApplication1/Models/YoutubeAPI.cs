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
        public async Task<List<string>> IndexYoutube(string param)
        {
            List<string> testa = new List<string>();
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyCYxw2GRHzN_UUP5kCOSBKuKAIfCdpd3ws"
                });

                // 動画IDを入力
                string liveChatId = GetLiveChatID(param, youtubeService);
                testa = await GetLiveChatMessage(liveChatId, youtubeService, null);
                 
            }
            catch(Exception)
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
        static public async Task<List<string>> GetLiveChatMessage(string liveChatId, YouTubeService youtubeService, string nextPageToken)
        {
            string liveMsg = null;
            List<string> liveMessagesList = new List<string>();

            var liveChatRequest = youtubeService.LiveChatMessages.List(liveChatId, "snippet,authorDetails");
            liveChatRequest.PageToken = nextPageToken;

            Google.Apis.YouTube.v3.Data.LiveChatMessageListResponse liveChatResponse = liveChatRequest.Execute();
            liveChatInfo lcInfo = new liveChatInfo();

            // ループ処理をして文字列型リストに格納
            foreach (var liveChat in liveChatResponse.Items)
            {
                try
                {
                    //Console.WriteLine($"{liveChat.Snippet.DisplayMessage},{liveChat.AuthorDetails.DisplayName}");

                    lcInfo.dspMessage = liveChat.Snippet.DisplayMessage;
                    lcInfo.dspName = liveChat.AuthorDetails.DisplayName;

                    //lcInfo.dspMessagesList.Add(lcInfo.dspMessage);
                    //lcInfo.dspNamesList.Add(lcInfo.dspName);
                    liveMsg = "名前【" + lcInfo.dspName + "】" + "\r\n" + "メッセージ『" +lcInfo.dspMessage + "』\t";
                    liveMessagesList.Add(liveMsg);
                }
                catch
                {
                    // 例外処理
                }
            }
            await Task.Delay((int)liveChatResponse.PollingIntervalMillis);


            //await GetLiveChatMessage(liveChatId, youtubeService, liveChatResponse.NextPageToken);
            return liveMessagesList;
        }
    }
}