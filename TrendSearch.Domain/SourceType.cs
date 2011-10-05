using System;

namespace TrendSearch.Domain
{
    /// <summary>
    /// The different source types supported by the application, so far
    /// </summary>
    public enum SourceType
    {
        Twitter, YouTube, RSS
    }

    public static class SourceTypeExtensions
    {
        public static string IconURL(this SourceType pType)
        {
            switch (pType)
            {
                case SourceType.RSS: return new System.Configuration.AppSettingsReader().GetValue("rss_icon_url", typeof(string)).ToString();
                case SourceType.Twitter: return new System.Configuration.AppSettingsReader().GetValue("twitter_icon_url", typeof(string)).ToString();
                case SourceType.YouTube: return new System.Configuration.AppSettingsReader().GetValue("youtube_icon_url", typeof(string)).ToString();
                default: throw new Exception("Unrecognized source type");
            }
        }
    }
}