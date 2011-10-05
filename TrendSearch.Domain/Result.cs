using System;

namespace TrendSearch.Domain
{
    public class Result
    {
        /// <summary>
        /// Title, if any
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Description text
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Browsing URL
        /// </summary>
        public string URL { get; set; }
        
        /// <summary>
        /// Rating over the search key word
        /// </summary>
        public decimal Rating { get; set; }
        
        /// <summary>
        /// Creation date at the source
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        
        /// <summary>
        /// Icon URL for the results list
        /// </summary>
        public string IconURL { get; set; }
    }
}