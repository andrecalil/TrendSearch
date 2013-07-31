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

        public RssSource(int pMaxResultSearch): base(pMaxResultSearch)
        { }

        [XmlAttribute("url")]
        public string URL { get; set; }

        public override List<Result> Search(string pKeyWords)
        {
            RssDataSource mRssDataSource = new RssDataSource()
            {
                Url = this.URL,
                MaxItems = this.MaxResultSearch
            };

            mRssDataSource.Rss.SelectItems();

            List<Result> mDomainResults = new List<Result>();

            StringBuilder mCompleteResult;

            foreach (RssItem mRssItem in mRssDataSource.Rss.Channel.Items)
            {
                mCompleteResult = new StringBuilder();
                mCompleteResult.AppendFormat("{0} ",mRssItem.Title);
                mCompleteResult.AppendFormat("{0} ", mRssItem.Description);
                mCompleteResult.AppendFormat("{0} ", mRssItem.Categories);

                if (this.ResultHitsSearch(mCompleteResult.ToString(), pKeyWords))
                {
                    mDomainResults.Add(
                        new Result()
                        {
                            CreatedDate = mRssItem.PubDateParsed,
                            Type = SourceType.RSS,
                            Text = mRssItem.Description,
                            Title = mRssItem.Title,
                            URL = mRssItem.Link
                        }
                    );
                }
            }

            return mDomainResults;
        }
    }
}