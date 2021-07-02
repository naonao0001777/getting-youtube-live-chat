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

        /// <summary>
        /// ページトークン生成最大件数
        /// </summary>
        public static int maxCountCreatePageToken = 7;

        /// <summary>
        /// APIキー
        /// </summary>
        private const string API_KEY = "AIzaSyARlEuRMw7DIRcgpuC-18TRTHqcwCxTaNc";

        public async Task<LiveChatModelList> IndexYoutube(string param, string quantity)
        {
            LiveChatModelList lcmL = new LiveChatModelList();
            try
            {
                // APIキーからYouTubeのサービスを取得する
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = API_KEY
                });

                // YoutubeLiveの特殊IDを取得する
                string liveChatId = GetLiveChatID(param, youtubeService);

                // 取得したメッセージを返す
                lcmL = await GetLiveChatMessage(liveChatId, youtubeService, null, quantity);
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
            }

            return lcmL;
        }

        /// <summary>
        /// 動画IDを取得する
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="youTubeService"></param>
        /// <returns></returns>
        static public string GetLiveChatID(string videoId, YouTubeService youTubeService)
        {
            // ライブの詳細を取得する
            var videosList = youTubeService.Videos.List("LiveStreamingDetails");

            videosList.Id = videoId;

            var videoListResponse = videosList.Execute();

            foreach (var videoID in videoListResponse.Items)
            {
                return videoID.LiveStreamingDetails.ActiveLiveChatId;
            }

            return null;
        }

        /// <summary>
        /// 動画コメントを取得する
        /// </summary>
        /// <param name="liveChatId"></param>
        /// <param name="youtubeService"></param>
        /// <param name="nextPageToken"></param>
        /// <returns></returns>
        static public async Task<LiveChatModelList> GetLiveChatMessage(string liveChatId, YouTubeService youtubeService, string nextPageToken_, string quantity)
        {
            // 結果を入れるリスト
            LiveChatModelList list = new LiveChatModelList();

            // 一時的なリスト
            List<LiveChatModel> instantList = new List<LiveChatModel>();

            // レスポンスリスト
            Google.Apis.YouTube.v3.Data.LiveChatMessageListResponse liveChatResponse;

            // LiveChatMessageトークンを取得する
            var liveChatRequest = youtubeService.LiveChatMessages.List(liveChatId, "snippet,authorDetails");
            
            maxCountCreatePageToken = int.Parse(quantity)/75;

            // 最終ページトークンまで再帰的な取得をする
            for (int i = 0; i < maxCountCreatePageToken; i++)
            {
                // 次回ページトークン
                liveChatRequest.PageToken = nextPageToken_;
                
                // リクエストする
                liveChatResponse = liveChatRequest.Execute();

                if (liveChatResponse.Items.Count == 0)
                {
                    break;
                }

                // 再帰的にチャットを取得
                foreach (var liveChat in liveChatResponse.Items)
                {
                    try
                    {
                        LiveChatModel lcInfo = new LiveChatModel();
                        lcInfo.DspMessage = liveChat.Snippet.DisplayMessage;
                        lcInfo.DspName = liveChat.AuthorDetails.DisplayName;
                        lcInfo.ChannelUrl = liveChat.AuthorDetails.ChannelUrl;
                        lcInfo.ChatDateTime = liveChat.Snippet.PublishedAt;
                        lcInfo.ProfileImageUrl = liveChat.AuthorDetails.ProfileImageUrl;
                        
                        instantList.Add(lcInfo);

                    }
                    catch (Exception ext)
                    {
                        string errMessage = ext.Message;
                        Console.Write(errMessage);
                    }
                }

                list.ChatList = instantList;

                // YouTubeのサービスから反応が返ってくるまで待ち
                await Task.Delay((int)liveChatResponse.PollingIntervalMillis);

                // ページトークンを確保する（初回はnull）
                nextPageToken_ = liveChatResponse.NextPageToken;
            }
            return list;

        }

    }
}