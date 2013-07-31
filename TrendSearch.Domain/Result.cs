using System;

namespace TrendSearch.Domain
{
    public class Result
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public decimal Rating { get; set; }
        public DateTime? CreatedDate { get; set; }
        public SourceType Type { get; set; }
    }
}