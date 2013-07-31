using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TrendSearch.Domain.Sources;

namespace TrendSearch.Domain
{
    [XmlRoot("searchSettings")]
    public class Search
    {
        public Search()
        {
            this.Sources = new List<BaseSource>();
        }

        [XmlElement("keyWords")]
        public string KeyWords { get; set; }
        
        [XmlAttribute("createdOn")]
        public DateTime SearchedDate { get; set; }

        [XmlElement("source")]
        public List<BaseSource> Sources { get; set; }

        public List<Result> SearchAndReate()
        {
            List<Result> mRatedResults = new List<Result>();

            foreach (BaseSource mSource in this.Sources)
                mRatedResults.AddRange(mSource.SearchAndRate(this.KeyWords));

            return mRatedResults.OrderByDescending<Result, decimal>(x => x.Rating).ToList<Result>();
        }
    }
}