using System.Collections.Generic;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    public static class Websites
    {
        public static readonly Dictionary<string, HashSet<string>> Categories = new()
        {
            { "Social Media", new() { "facebook", "twitter", "instagram", "tiktok", "reddit", "linkedin", "snapchat", "threads" } },
            { "Streaming", new() { "youtube", "twitch", "netflix", "hulu", "disney+", "prime video", "spotify", "crunchyroll", "hbomax" } },
            { "Shopping", new() { "amazon", "ebay", "aliexpress", "etsy", "walmart", "bestbuy", "target", "newegg" } },
            { "Work/Development", new() { "github", "gitlab", "stackoverflow", "notion", "google docs", "google drive", "dropbox", "trello", "jira" } },
            { "News", new() { "bbc", "cnn", "ny times", "the guardian", "fox news", "reuters", "al jazeera", "forbes" } },
            { "AI Tools", new() { "chatgpt", "midjourney", "dall·e", "bard", "runway ml", "stable diffusion" } },
            { "Finance", new() { "paypal", "bank", "tradingview", "coinbase", "binance", "robinhood", "yahoo finance" } },
            { "Education", new() { "khan academy", "coursera", "udemy", "duolingo", "wikipedia", "edx", "codecademy" } }
        };
    }
}
