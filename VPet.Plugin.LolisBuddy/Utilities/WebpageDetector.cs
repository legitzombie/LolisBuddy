using System.Collections.Generic;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    public class WebpageDetector
    {

        public static List<string> Categorize(string windowTitle, string Category = "Browser")
        {

            windowTitle = windowTitle.ToLower();

            foreach (var category in Websites.Categories)
            {
                foreach (var keyword in category.Value)
                {
                    if (windowTitle.Contains(keyword))
                    {
                        Category = category.Key;
                        windowTitle = keyword;
                    }
                }
            }

            List<string> info = new List<string>();
            info.Add(windowTitle);
            info.Add(Category);

            return info;
        }


    }
}
