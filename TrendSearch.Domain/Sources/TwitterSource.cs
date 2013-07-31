using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LinqToTwitter;

namespace TrendSearch.Domain.Sources
{
    public class TwitterSource : BaseSource
    {
        public TwitterSource()
        { }

        public TwitterSource(int pMaxResultSearch): base(pMaxResultSearch)
        { }

        public override List<Result> Search(string pKeyWords)
        {
            string twitterConsumerKey = ConfigurationManager.AppSettings["twitter-consumer-key"];
            string twitterConsumerSecret = ConfigurationManager.AppSettings["twitter-consumer-secret"];

            if (string.IsNullOrWhiteSpace(twitterConsumerKey) || string.IsNullOrWhiteSpace(twitterConsumerSecret))
                throw new Exception("App was unable to find Twitter credentials on the current settings file. Please add twitter-consumer-key and twitter-consumer-secret to the appSettings section.");

            ApplicationOnlyAuthorizer authorization = new ApplicationOnlyAuthorizer()
            {
                Credentials = new InMemoryCredentials()
                {
                    ConsumerKey = twitterConsumerKey,
                    ConsumerSecret = twitterConsumerSecret
                }
            };

            authorization.Authorize();

            if(!authorization.IsAuthorized)
                throw new Exception("Twitter authorizaton was unsuccessful. Please review your Twitter key and secret.");

            TwitterContext twitterContext = new TwitterContext(authorization);

            LinqToTwitter.Search twitterSearch =
                (from search in twitterContext.Search
                 where search.Type == SearchType.Search &&
                       search.Query == pKeyWords &&
                       search.Count == this.MaxResultSearch
                 select search)
                .SingleOrDefault();

            IEnumerable<Status> tweets =
                from status in twitterSearch.Statuses
                orderby status.CreatedAt descending
                select status;

            List<Result> domainResults = new List<Result>();

            foreach (Status status in tweets)
            {
                domainResults.Add(
                    new Result()
                    {
                        CreatedDate = status.CreatedAt,
                        Type = SourceType.Twitter,
                        Text = status.Text,
                        Title = status.Text.Length > 50? string.Format("{0}...",status.Text.Substring(0, 47)) : status.Text,
                        URL = string.Format("http://twitter.com/{0}", status.User.Identifier.ScreenName)
                    }
                );
            }

            return domainResults;
        }
    }
}