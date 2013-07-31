using System.Collections.Generic;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.YouTube;
using System.Xml.Serialization;
using System.Configuration;
using System;

namespace TrendSearch.Domain.Sources
{
    public class YouTubeSource : BaseSource
    {
        public YouTubeSource()
        {}

        public YouTubeSource(int pMaxResultSearch): base(pMaxResultSearch)
        {}

        public override List<Result> Search(string pKeyWords)
        {
            string youtubeApplicationName = ConfigurationManager.AppSettings["youtube-application-name"];
            string youtubeDeveloperKey = ConfigurationManager.AppSettings["youtube-developer-key"];

            if (string.IsNullOrWhiteSpace(youtubeApplicationName) || string.IsNullOrWhiteSpace(youtubeDeveloperKey))
                throw new Exception("App was unable to find Youtube credentials on the current settings file. Please add youtube-application-name and youtube-developer-key to the appSettings section.");

            YouTubeRequestSettings requestSettings = new YouTubeRequestSettings(youtubeApplicationName, youtubeDeveloperKey);
            requestSettings.AutoPaging = false;

            YouTubeRequest youtubeRequest = new YouTubeRequest(requestSettings);

            YouTubeQuery youtubeQuery = new YouTubeQuery(YouTubeQuery.DefaultVideoUri)
            {
                OrderBy = "viewCount",
                Query = pKeyWords,
                NumberToRetrieve = this.MaxResultSearch
            };

            Feed<Video> youtubeVideos = youtubeRequest.Get<Video>(youtubeQuery);

            List<Result> domainResults = new List<Result>();

            foreach (Video video in youtubeVideos.Entries)
            {
                domainResults.Add(
                    new Result()
                    {
                        CreatedDate = video.Updated,
                        Type = SourceType.YouTube,
                        Text = string.Format("{0} {1}", video.Description, video.Keywords),
                        Title = video.Title,
                        URL = video.WatchPage.OriginalString
                    }
                );
            }

            return domainResults;
        }
    }
}