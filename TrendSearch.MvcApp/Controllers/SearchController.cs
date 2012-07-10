using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrendSearch.Domain;

namespace TrendSearch.MvcApp.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Auto(string keyWord)
        {
            this.SetSearch(keyWord, 20, 20);

            return Redirect("/Results");
        }

        [HttpPost]
        public ActionResult Index(string keyWords, string searchTwitter, string searchYouTube, int? maxTweets, int? maxVideos)
        {
            int? mMaxTweets = null;
            int? mMaxVideos = null;

            if (!string.IsNullOrEmpty(searchTwitter) && searchTwitter.Equals("on"))
            {
                mMaxTweets = maxTweets.HasValue ? maxTweets.Value : 20;
            }

            if (!string.IsNullOrEmpty(searchYouTube) && searchYouTube.Equals("on"))
            { 
                mMaxVideos = maxVideos.HasValue ? maxVideos.Value : 20; 
            }

            this.SetSearch(keyWords, mMaxTweets, mMaxVideos);

            return Redirect("/Results");
        }

        private void SetSearch(string pKeyWords, int? pMaxTweets, int? pMaxVideos)
        {
            Search mCurrentSearch = new Search();
            mCurrentSearch.KeyWords = pKeyWords;
            mCurrentSearch.SearchedDate = DateTime.Now;

            if (pMaxTweets.HasValue)
            {
                mCurrentSearch.Sources.Add(new TwitterSource(pMaxTweets.Value));
            }

            if (pMaxVideos.HasValue)
            {
                mCurrentSearch.Sources.Add(new YouTubeSource(pMaxVideos.Value));
            }

            this.Session.Add("currentSearch", mCurrentSearch);
        }
    }
}