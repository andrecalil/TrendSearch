using System.Collections.Generic;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.YouTube;
using System.Xml.Serialization;

namespace TrendSearch.Domain
{
    public class YouTubeSource : Source
    {
        private YouTubeRequest aRequest;

        public YouTubeSource()
        {
            this.InstantiateYouTubeRequest();
        }

        public YouTubeSource(int pMaxResultSearch)
            : base(pMaxResultSearch)
        {
            this.InstantiateYouTubeRequest();   
        }

        private void InstantiateYouTubeRequest()
        {
            //This is how an application integrates with YouTube. You must have a developer key (that hash over there)
            YouTubeRequestSettings mRequestSettings = new YouTubeRequestSettings("TrendSearch", "AI39si4L15_ODlB304zllvgYaSXyu0KRvX-yE6c3Yu5ihR_lcWIf5abwh65tP7sr-HR7gxMdNl4pdT00-F7F8-sWHLVJW7ZGhw");
            mRequestSettings.AutoPaging = false;

            this.aRequest = new YouTubeRequest(mRequestSettings);
        }

        public override List<Result> Search(string pKeyWords)
        {
            #region Retrieve videos

            YouTubeQuery mYouTubeQuery = new YouTubeQuery(YouTubeQuery.DefaultVideoUri)
            {
                OrderBy = "viewCount",
                Query = pKeyWords,
                NumberToRetrieve = this.MaxResultSearch
            };

            Feed<Video> mVideosResult = this.aRequest.Get<Video>(mYouTubeQuery);

            #endregion

            List<Result> mDomainResults = new List<Result>();

            #region Convert videos to results

            foreach (Video mVideo in mVideosResult.Entries)
            {
                mDomainResults.Add(
                    new Result()
                    {
                        CreatedDate = mVideo.Updated,
                        IconURL = SourceType.YouTube.IconURL(),
                        Text = string.Format("{0} {1}", mVideo.Description, mVideo.Keywords),
                        Title = mVideo.Title,
                        URL = mVideo.WatchPage.OriginalString
                    }
                    );
            }

            #endregion

            return mDomainResults;
        }
    }
}