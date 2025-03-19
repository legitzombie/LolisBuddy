using System.Collections.Generic;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.Sys
{
    public class ProcessesManager
    {
        public ProcessesManager() { }

        public static List<string> Categorize(string processName, string windowTitle)
        {
            processName = processName.ToLower();
            windowTitle = windowTitle.ToLower();

            List<string> info = new List<string>();
            string category = "Uncategorized";
            string title = windowTitle; // Default to the original title



            // Iterate through category mappings
            foreach (var entry in Processes.Categories)
            {
                foreach (var keyword in entry.Value)
                {
                    if (processName.Contains(keyword))
                    {
                        if (entry.Key == "Browser")
                        {
                            List<string> webpage = WebpageDetector.Categorize(windowTitle);
                            category = webpage[1]; title = webpage[0];
                        }
                        else
                        {
                            category = entry.Key;
                            title = keyword; // Set the title to the matched keyword
                        }
                        return new List<string> { category, title }; // Return immediately after the first match
                    }
                }
            }

            // Prioritize detecting a game first
            if (GameDetector.HasGameDLLs(processName) && category == "Uncategorized" && !IsBlacklisted(processName))
            {
                category = "Game";
            }


            return new List<string> { category, title };
        }


        public static bool IsBlacklisted(string name)
        {
            return Processes.Windows.Contains(name.ToLower());
        }

    }


}
