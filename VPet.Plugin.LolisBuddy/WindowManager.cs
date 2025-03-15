using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using LinePutScript.Converter;
using System.Windows;
using System.Reflection;
using System.IO;

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

        public List<ActiveWindow> Windows = new List<ActiveWindow>(); // Store window history
        public ActiveWindow window { get; private set; } = new ActiveWindow(); // Ensure it's initialized
        public ProcessesManager processesManager = new ProcessesManager();

        private IOManager iOManager = new IOManager();

        public WindowManager()
        {
            Windows = iOManager.LoadLPS<ActiveWindow>(MemoryPath,"memory");
        }

        /// <summary>
        /// Gets details of the currently active process (name, window title, and uptime).
        /// </summary>
        public void UpdateActiveWindowDetails()
        {
            IntPtr hWnd = GetForegroundWindow(); // Get active window handle

            if (hWnd == IntPtr.Zero)
                return; // No active window found

            // Get process ID from window handle
            GetWindowThreadProcessId(hWnd, out int processId);

            try
            {
                Process proc = Process.GetProcessById(processId);

                // Extract details
                string processName = proc.ProcessName ?? "Unknown"; // Example: "chrome"
                string windowTitle = GetActiveWindowTitle(hWnd);
                TimeSpan uptime = GetProcessUptime(proc);

                if (processesManager.IsBlacklisted(processName)) return;

                // Check if the window already exists in the list
                ActiveWindow existingWindow = Windows.Find(w => w.Process == processName && w.Title == windowTitle);

                if (existingWindow != null)
                {
                    // Update uptime if the window already exists
                    existingWindow.Runtime = uptime.Minutes;
                }
                else
                {
                    // Otherwise, add a new entry
                    var newWindow = new ActiveWindow
                    {
                        Process = processName,
                        Title = windowTitle,
                        Runtime = uptime.Minutes
                    };
                    Windows.Add(newWindow);
                }

                // Update the currently active window reference
                window.Process = processName;
                window.Title = windowTitle;
                window.Runtime = uptime.Minutes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving process details: {ex.Message}");
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
    }
}
