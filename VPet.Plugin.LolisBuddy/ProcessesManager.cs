using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPet.Plugin.LolisBuddy
{
    public class ProcessesManager
    {
        public ProcessesManager() { }

        private static readonly HashSet<string> BlacklistedProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "explorer",          // File Explorer
            "taskmgr",           // Task Manager
            "shellexperiencehost", // Windows Shell (Start Menu, etc.)
            "runtimebroker",     // Windows Process
            "searchui",          // Windows Search Bar
            "systemsettings",    // Windows Settings
            "applicationframehost", // UWP App Host
            "dwm",              // Desktop Window Manager
            "sihost",           // Shell Infrastructure Host
            "ctfmon",           // Input Language, Text Input
            "winlogon",         // Windows Login UI
            "smartscreen",      // Windows Defender SmartScreen
            "securityhealthsystray", // Windows Security
            "startmenuexperiencehost", // Start Menu Process
            "gamebarpresencewriter", // Xbox Game Bar
            "textinputhost",    // On-screen keyboard, input processes
        };
        public bool IsBlacklisted(string processName)
        {
            return BlacklistedProcesses.Contains(processName);
        }
    }
}
