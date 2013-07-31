using System;
using TrendSearch.Domain.Sources;

namespace TrendSearch.Domain
{
    public static class SourceFactory
    {
        /// <summary>
        /// Instantiates the selected type of source
        /// </summary>
        /// <param name="pType">Source type</param>
        /// <param name="pMaxResultSearch">Max results to bring from the source</param>
        /// <returns></returns>
        public static BaseSource GetSource(SourceType pType, int pMaxResultSearch)
        {
            switch (pType)
            {
                case SourceType.Twitter: return new TwitterSource(pMaxResultSearch);
                case SourceType.YouTube: return new YouTubeSource(pMaxResultSearch);
                case SourceType.RSS: return new RssSource(pMaxResultSearch);
                default: throw new Exception("Unrecognized source type");
            }
        }
    }
}