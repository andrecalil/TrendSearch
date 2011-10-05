using System.Collections.Generic;
using System.Text;
using RssToolkit.Rss;
using RssToolkit.Web.WebControls;
using System.Xml.Serialization;

namespace TrendSearch.Domain
{
    public class RssSource : Source
    {
        public RssSource() { }

        public RssSource(int pMaxResultSearch)
            : base(pMaxResultSearch)
        { }

        [XmlAttribute("url")]
        public string URL { get; set; }

        public override List<Result> Search(string pKeyWords)
        {
            #region Retrieve the posts

            RssDataSource mRssDataSource = new RssDataSource()
            {
                Url = this.URL,
                MaxItems = this.MaxResultSearch
            };

            mRssDataSource.Rss.SelectItems();

            #endregion

            List<Result> mDomainResults = new List<Result>();

            bool mResultHitsSearch;
            StringBuilder mCompleteResult;

            foreach (RssItem mRssItem in mRssDataSource.Rss.Channel.Items)
            {
                #region Result hits search keywords?

                mCompleteResult = new StringBuilder();
                mCompleteResult.AppendFormat("{0} ",mRssItem.Title);
                mCompleteResult.AppendFormat("{0} ", mRssItem.Description);
                mCompleteResult.AppendFormat("{0} ", mRssItem.Categories);

                mResultHitsSearch = this.ResultHitsSearch(mCompleteResult.ToString(), pKeyWords);

                #endregion

                #region If so, create the result

                if (mResultHitsSearch)
                {
                    mDomainResults.Add(
                        new Result()
                        {
                            CreatedDate = mRssItem.PubDateParsed,
                            IconURL = SourceType.RSS.IconURL(),
                            Text = mRssItem.Description,
                            Title = mRssItem.Title,
                            URL = mRssItem.Link
                        }
                    );
                }

                #endregion
            }

            return mDomainResults;
        }
    }
}