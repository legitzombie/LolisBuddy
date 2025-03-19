using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LinePutScript;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy
{
    public class WindowManager
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow(); // Get active window handle

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId); // Get process ID

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count); // Get window title

        public List<ActiveWindow> windows = new List<ActiveWindow>(); // Store window history
        public ActiveWindow window { get; private set; } = new ActiveWindow(); // Ensure it's initialized
        public ProcessesManager processesManager = new ProcessesManager();

        private IOManager iOManager = new IOManager();

        public WindowManager()
        {
            windows = iOManager.LoadLPS<ActiveWindow>(AIManager.MemoryTypePath(AIManager.ActionMemoryPath), null, true, true);
        }

        public void CategorizeBehavior()
        {
            List<ProgramBehavior> behavior = new List<ProgramBehavior>();

            // Dictionary to track unique categories and their total runtime
            Dictionary<string, int> categoryRuntimeMap = new Dictionary<string, int>();

            foreach (var window in windows)
            {
                string category = window.Category;
                if (category == "Uncategorized") continue;
                if (WebpageDetector.WebsiteCategories.ContainsKey(category)) category = window.Title;
                if (categoryRuntimeMap.ContainsKey(category))
                {
                    categoryRuntimeMap[category] += window.Runtime;
                }
                else
                {
                    categoryRuntimeMap[category] = window.Runtime;
                }
            }

            // Convert dictionary to a sorted list of UserBehavior objects (descending by runtime)
            behavior = categoryRuntimeMap
                .OrderByDescending(entry => entry.Value)
                .Select(entry => new ProgramBehavior
                {
                    Category = entry.Key,
                    Runtime = entry.Value
                })
                .ToList();

            iOManager.SaveLPS(behavior, AIManager.BehaviorMemoryFolderPath, "preferences", true);
        }



        /// <summary>
        /// Gets details of the currently active process (name, window title, and uptime).
        /// </summary>
        public void UpdateActiveWindowDetails()
        {
            IntPtr hWnd = GetForegroundWindow(); // Get active window handle

            if (hWnd == IntPtr.Zero)
            {
                return;
            }

            GetWindowThreadProcessId(hWnd, out int processId);

            try
            {
                Process proc = Process.GetProcessById(processId);

                // Extract details
                string processName = proc?.ProcessName ?? "Unknown";
                string windowTitle = GetActiveWindowTitle(hWnd);
                string date = DateTime.Now.ToString();
                List<string> info = processesManager.Categorize(processName, windowTitle);
                string category = info[0];
                windowTitle = info[1];

                // Update window details
                window.Title = windowTitle;
                window.Category = category;
                window.Date = date;
                window.Process = processName;
                window.Runtime += 2000; // Increment by 1 minute for categorized websites


                // Find an existing window entry
                ActiveWindow existingWindow = WebpageDetector.WebsiteCategories.ContainsKey(category)
                    ? windows.Find(w => w.Category == category) // Match by category for websites
                    : windows.Find(w => w.Process == processName && w.Title == windowTitle); // Match process & title for apps

                if (existingWindow != null)
                {
                    existingWindow.Date = date;
                    existingWindow.Runtime += 2000; // Increment runtime for categorized websites

                }
                else
                {
                    int run = 0;

                    windows.Add(new ActiveWindow
                    {
                        Process = processName,
                        Title = windowTitle,
                        Runtime = run,
                        Date = date,
                        Category = category
                    });

                    // Remove duplicate entries (keeping the first occurrence)
                    windows = windows.GroupBy(w => new { w.Process, w.Title })
                                     .Select(g => g.First())
                                     .ToList();
                }

                // Save the updated list
                iOManager.SaveLPS(windows, AIManager.MemoryTypePath(AIManager.ActionMemoryPath, false), null, true);
                iOManager.SaveLPS(windows, AIManager.MemoryTypePath(AIManager.ActionMemoryPath, true), null, true);
                CategorizeBehavior();
            }
            catch (IOException)
            {

            }
        }

        /// <summary>
        /// Gets the window title of the given window handle.
        /// </summary>
        private static string GetActiveWindowTitle(IntPtr hWnd)
        {
            StringBuilder title = new StringBuilder(256);
            GetWindowText(hWnd, title, 256);
            return title.ToString();
        }
    }

    public class ActiveWindow
    {
        [Line] public string Title { get; set; }
        [Line] public string Process { get; set; }
        [Line] public int Runtime { get; set; }
        [Line] public string Date { get; set; }
        [Line] public string Category { get; set; }
    }

    public class ProgramBehavior
    {
        [Line] public string Category { get; set; }
        [Line] public int Runtime { get; set; }
    }
}
