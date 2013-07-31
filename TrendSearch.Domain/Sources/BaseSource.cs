using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TrendSearch.Domain.Sources
{
    [XmlInclude(typeof(YouTubeSource))]
    [XmlInclude(typeof(TwitterSource))]
    [XmlInclude(typeof(RssSource))]
    [XmlRoot("source")]
    public abstract class BaseSource
    {
        public BaseSource() { }

        public BaseSource(int pMaxResultSearch)
        {
            this.MaxResultSearch = pMaxResultSearch;
        }

        [XmlAttribute("maxSearch")]
        public int MaxResultSearch { get; set; }

        public abstract List<Result> Search(string pKeyWords);

        protected void RateResult(Result pResultToBeRated, string pKeyWords)
        {
            string[] mNormalizedKeyWords = this.NormalizeKeyWords(pKeyWords);
            string mNormalizedContent = string.Format("{0} {1}", pResultToBeRated.Title, pResultToBeRated.Text);

            bool mOrderedHits = true;
            int mLastHitIndex = int.MinValue;
            foreach(string mKey in mNormalizedKeyWords)
            {
                if(mNormalizedContent.Contains(mKey))
                {
                    pResultToBeRated.Rating += 1;

                    if (mOrderedHits && mNormalizedContent.IndexOf(mKey) > mLastHitIndex)
                        mLastHitIndex = mNormalizedContent.IndexOf(mKey);
                    else
                        mOrderedHits = false;
                }
            }

            if (mOrderedHits)
                pResultToBeRated.Rating += 1;
        }

        /// <summary>
        /// Weak keywords normalization. Only verify if it's not empty, take all strings to lower case
        /// and splits on the empty spaces
        /// </summary>
        /// <param name="pKeyWords"></param>
        /// <returns></returns>
        protected string[] NormalizeKeyWords(string pKeyWords)
        {
            return string.IsNullOrEmpty(pKeyWords) ? new string[] { string.Empty } : pKeyWords.ToLower().Split(' ');
        }

        protected bool ResultHitsSearch(string pCompleteResult, string pKeyWords)
        {
            return this.NormalizeKeyWords(pKeyWords).All(x => pCompleteResult.ToLower().Contains(x));
        }

        /// <summary>
        /// Searches for the given keywords on the instantiated source and rate each result based on the keywords
        /// </summary>
        /// <param name="pKeyWords">Keywords to be used for the search</param>
        /// <returns>List of the results found</returns>
        public List<Result> SearchAndRate(string pKeyWords)
        {
            List<Result> results = this.Search(pKeyWords);

            foreach (Result result in results)
                this.RateResult(result, pKeyWords);

            return results;
        }
    }
}