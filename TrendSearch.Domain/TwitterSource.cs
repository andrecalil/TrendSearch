using System.Collections.Generic;
using Twitterizer;
using System.Xml.Serialization;

namespace TrendSearch.Domain
{
    public class TwitterSource : Source
    {
        public TwitterSource() { }

        public TwitterSource(int pMaxResultSearch)
            : base(pMaxResultSearch)
        {
        }

        public override List<Result> Search(string pKeyWords)
        {
            #region Retrieve tweets

            SearchOptions mTwitterSearchOptions = new SearchOptions()
            {
                PageNumber = 1,
                NumberPerPage = this.MaxResultSearch
            };

            TwitterResponse<TwitterSearchResultCollection> mTwitterSearchResult = TwitterSearch.Search(pKeyWords, mTwitterSearchOptions);

            #endregion

            List<Result> mDomainResults = new List<Result>();

            #region Convert to results

            if (mTwitterSearchResult.Result == RequestResult.Success)
            {
                foreach (TwitterSearchResult mTweet in mTwitterSearchResult.ResponseObject)
                {
                    mDomainResults.Add(
                        new Result()
                        {
                            CreatedDate = mTweet.CreatedDate,
                            IconURL = SourceType.Twitter.IconURL(),
                            Text = mTweet.Text,
                            Title = mTweet.Text.Length > 50? string.Format("{0}...",mTweet.Text.Substring(0, 47)) : mTweet.Text,
                            URL = string.Format("http://twitter.com/{0}",mTweet.FromUserScreenName)
                        }
                    );
                }
            }

            #endregion

            return mDomainResults;
        }
    }
}