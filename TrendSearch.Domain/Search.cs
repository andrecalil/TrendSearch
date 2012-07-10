using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TrendSearch.Domain
{
    [XmlRoot("searchSettings")]
    public class Search
    {
        public Search()
        {
            this.Sources = new List<Source>();
        }

        [XmlElement("keyWords")]
        public string KeyWords { get; set; }
        
        [XmlAttribute("createdOn")]
        public DateTime SearchedDate { get; set; }

        [XmlElement("source")]
        public List<Source> Sources { get; set; }

        /// <summary>
        /// Concatenates the results from each source and orders by the rating and creation date
        /// </summary>
        /// <returns></returns>
        public List<Result> SearchAndReate()
        {
            List<Result> mRatedResults = new List<Result>();

            foreach (Source mSource in this.Sources)
            {
                mRatedResults.AddRange(mSource.SearchAndRate(this.KeyWords));
            }

            return mRatedResults.OrderByDescending<Result, decimal>(x => x.Rating).ToList<Result>();
        }
    }
}