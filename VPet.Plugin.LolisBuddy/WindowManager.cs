using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using LinePutScript;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy
{
    public class WindowManager
    {

        private static readonly string MemoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
            windows = iOManager.LoadLPS<ActiveWindow>(MemoryPath, "memory", true);
        }

        private string GetBestAppName(string processName, string windowTitle, Dictionary<string, HashSet<string>> categoryMapping)
        {
            foreach (var category in categoryMapping)
            {
                foreach (var knownApp in category.Value)
                {
                    if (processName.IndexOf(knownApp, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(knownApp.Replace("_", " "));
                    }
                }
            }

            // If no known app is found, clean up the window title
            string cleanedTitle = ExtractRelevantTitle(windowTitle);
            return !string.IsNullOrWhiteSpace(cleanedTitle) ? cleanedTitle : "this program";
        }

        private readonly HashSet<string> ignoredTitles = new() { "mainwindow", "application", "untitled", "new document", "welcome screen" };

        private string ExtractRelevantTitle(string windowTitle)
        {
            if (string.IsNullOrWhiteSpace(windowTitle) || windowTitle.Length < 3)
                return "this program";

            string lowerTitle = windowTitle.ToLower();
            if (ignoredTitles.Contains(lowerTitle))
                return "this program";

            // Remove clutter
            string cleaned = Regex.Replace(windowTitle, @"[\[\(].*?[\]\)]", ""); // Remove [Bracketed Text]
            cleaned = Regex.Replace(cleaned, @"[-–—:|].*", ""); // Remove everything after -, –, —, :, or |

            return cleaned.Trim();
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
                string processName = proc.ProcessName ?? "Unknown";
                string windowTitle = GetBestAppName(processName, GetActiveWindowTitle(hWnd), processesManager.CategoryMapping);
                TimeSpan uptime = GetProcessUptime(proc);
                string date = DateTime.Now.ToString();
                string category = processesManager.Categorize(processName, windowTitle)[0];
                string webpage = WebpageDetector.Categorize(GetActiveWindowTitle(hWnd));
                if (webpage != "") windowTitle = webpage;
                if (!processesManager.IsBlacklisted(processName))
                {
                    window.Title = windowTitle;
                    window.Category = category;
                    window.Date = date;
                    window.Runtime = uptime.Minutes;
                    window.Process = processName;
                }
                else return;

                    //MessageBox.Show($"App: {processName}.exe \n Title: {windowTitle} \n Category: {category} \n Usage: {uptime.Minutes}mins \n Last Used: {date}");


                ActiveWindow existingWindow = windows.Find(w => w.Process == processName);

                if (existingWindow != null)
                {

                    existingWindow.Runtime = uptime.Minutes;
                    existingWindow.Date = date;

                }
                else
                {


                    windows.Add(new ActiveWindow
                    {
                        Process = processName,
                        Title = windowTitle,
                        Runtime = uptime.Minutes,
                        Date = date,
                        Category = category
                    });

                    int beforeCount = windows.Count;
                    windows = windows.GroupBy(w => new { w.Process, w.Title })
                                     .Select(g => g.First())
                                     .ToList();
                    int afterCount = windows.Count;

                }


                iOManager.SaveLPS(windows, MemoryPath, "memory", true);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"[ERROR] Failed to retrieve process details: {ex.Message}");
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

        /// <summary>
        /// Safely gets the uptime of a process.
        /// </summary>
        private static TimeSpan GetProcessUptime(Process proc)
        {
            try
            {
                return DateTime.Now - proc.StartTime;
            }
            catch
            {
                return TimeSpan.Zero; // If access to StartTime is denied
            }
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
}
