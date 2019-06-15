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
        public async Task IndexYoutube(string param)
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyDZWkl8wyAeKlA7kxQYVaePk5uD6tegBm0"
                });

                // 動画IDを入力
                string liveChatId = GetLiveChatID(param, youtubeService);

                await GetLiveChatMessage(liveChatId, youtubeService, null);
            }
            catch(Exception e)
            {
               string errMessage = e.Message;
            }
            

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
        static public async Task GetLiveChatMessage(string liveChatId, YouTubeService youtubeService, string nextPageToken)
        {
            var liveChatRequest = youtubeService.LiveChatMessages.List(liveChatId, "snippet,authorDetails");
            liveChatRequest.PageToken = nextPageToken;

            var liveChatResponse = await liveChatRequest.ExecuteAsync();
            liveChatInfo lcInfo = new liveChatInfo();

            foreach (var liveChat in liveChatResponse.Items)
            {
                try
                {
                    Console.WriteLine($"{liveChat.Snippet.DisplayMessage},{liveChat.AuthorDetails.DisplayName}");

                    lcInfo.dspMessage = liveChat.Snippet.DisplayMessage;
                    lcInfo.dspName = liveChat.AuthorDetails.DisplayName;

                    lcInfo.dspMessagesList.Add(lcInfo.dspMessage);
                    lcInfo.dspNamesList.Add(lcInfo.dspName);
                    
                    
                }

                catch
                {
                    // 例外処理
                }

            }
            await Task.Delay((int)liveChatResponse.PollingIntervalMillis);


            await GetLiveChatMessage(liveChatId, youtubeService, liveChatResponse.NextPageToken);

        }
    }
}