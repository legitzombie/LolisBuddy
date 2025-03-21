using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LinePutScript;
using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Core;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.UI
{
    public class WindowManager
    {

        [DllImport("user32.dll")]
        private static extern nint GetForegroundWindow(); // Get active window handle

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(nint hWnd, out int processId); // Get process ID

        [DllImport("user32.dll")]
        private static extern int GetWindowText(nint hWnd, StringBuilder text, int count); // Get window title

        public static WindowEntry window = new WindowEntry();



        /// <summary>
        /// Gets details of the currently active process (name, window title, and uptime).
        /// </summary>
        public static void UpdateActiveWindowDetails()
        {
            nint hWnd = GetForegroundWindow(); // Get active window handle

            if (hWnd == nint.Zero) return;

            GetWindowThreadProcessId(hWnd, out int processId);


            try
            {

                Process proc = Process.GetProcessById(processId);

                // Extract details
                string processName = proc?.ProcessName ?? "Unknown";
                string windowTitle = GetActiveWindowTitle(hWnd);
                string date = DateTime.Now.ToString();
                List<string> info = ProcessesManager.Categorize(processName, windowTitle);
                string category = info[0];
                windowTitle = info[1];

                if (ProcessesManager.IsBlacklisted(processName)) return;

                // Update current window details
                window.Title = windowTitle;
                window.Category = category;
                window.Date = date;
                window.Process = processName;
                window.Runtime += 1000; // Increment runtime

                bool isWebsite = Websites.Categories.ContainsKey(category);

                // Find index of existing entry
                int existingIndex = AIManager.ProgramMemory.FindIndex(w => isWebsite ? w.Category == category : w.Process == processName);
                int shortExistingIndex = AIManager.ShortProgramMemory.FindIndex(w => isWebsite ? w.Category == category : w.Process == processName);


                if (existingIndex != -1)
                {
                    // Update runtime and date in the actual list
                    AIManager.ProgramMemory[existingIndex].Date = date;
                    AIManager.ProgramMemory[existingIndex].Runtime += 1000;
                }
                else
                {
                    // Add new entry if no match found
                    AIManager.ProgramMemory.Add(new WindowEntry
                    {
                        Process = processName,
                        Title = windowTitle,
                        Runtime = 1000,
                        Date = date,
                        Category = category
                    });
                }


                if (shortExistingIndex != -1)
                {
                    AIManager.ShortProgramMemory[shortExistingIndex].Date = date;
                    AIManager.ShortProgramMemory[shortExistingIndex].Runtime += 1000;
                }
                else
                {
                    AIManager.ShortProgramMemory.Add(new WindowEntry
                    {
                        Process = processName,
                        Title = windowTitle,
                        Runtime = 1000,
                        Date = date,
                        Category = category
                    });
                }

                // Save updated memory
                AIManager.Instance.saveMemory("actions");
            }
            catch (IOException)
            {

            }
        }

        public static List<WindowEntry> RemoveDuplicates(List<WindowEntry> memory)
        {
            return memory
                .GroupBy(w => Websites.Categories.ContainsKey(w.Category) ? w.Category : w.Process) // Group by category for websites, process for programs
                .Select(g => new WindowEntry
                {
                    Process = g.First().Process,
                    Title = g.First().Title,
                    Category = g.First().Category,
                    Runtime = g.Sum(w => w.Runtime),  // Sum runtime across duplicates
                    Date = g.Max(w => DateTime.Parse(w.Date)).ToString() // Keep the latest date
                })
                .ToList();
        }




        /// <summary>
        /// Gets the window title of the given window handle.
        /// </summary>
        private static string GetActiveWindowTitle(nint hWnd)
        {
            StringBuilder title = new StringBuilder(256);
            GetWindowText(hWnd, title, 256);
            return title.ToString();
        }
    }

    public class WindowEntry
    {
        [Line] public string Title { get; set; }
        [Line] public string Process { get; set; }
        [Line] public int Runtime { get; set; }
        [Line] public string Date { get; set; }
        [Line] public string Category { get; set; }
    }

}
