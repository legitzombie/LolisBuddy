using System.Collections.Generic;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.UI;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.Core
{
    public class AIManager
    {
        private static AIManager _instance;
        private static readonly object _lock = new object();

        public static AIManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AIManager();
                    return _instance;
                }
            }
        }

        public static List<DialogueEntry> SpeechMemory { get; private set; } = new List<DialogueEntry>();
        public static List<DialogueEntry> ShortSpeechMemory { get; private set; } = new List<DialogueEntry>();

        public static List<IdleEntry> IdleMemory { get; private set; } = new List<IdleEntry>();
        public static List<IdleEntry> ShortIdleMemory { get; private set; } = new List<IdleEntry>();

        public static List<ActiveWindow> ActionMemory { get; private set; } = new List<ActiveWindow>();
        public static List<ActiveWindow> ShortActionMemory { get; private set; } = new List<ActiveWindow>();

        public static List<WindowEntry> UserPreferences { get; private set; } = new List<WindowEntry>();
        public static List<IdleEntry> UserSleepSchedule { get; private set; } = new List<IdleEntry>();

        public void updateMemory()
        {
            SpeechMemory = IOManager.LoadLPS<DialogueEntry>(FolderPath.Get("memory", "speech", "long_term"), null, true, true);
            ShortSpeechMemory = IOManager.LoadLPS<DialogueEntry>(FolderPath.Get("memory", "speech", "short_term"), null, true, true);

            IdleMemory = IOManager.LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior", "sleep", "long_term"), null, true, true);
            ShortIdleMemory = IOManager.LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior", "sleep", "short_term"), null, true, true);

            ActionMemory = IOManager.LoadLPS<ActiveWindow>(FolderPath.Get("memory", "actions", "long_term"), null, true, true);
            ShortActionMemory = IOManager.LoadLPS<ActiveWindow>(FolderPath.Get("memory", "actions", "short_term"), null, true, true);

            UserPreferences = IOManager.LoadLPS<WindowEntry>(FolderPath.Get("memory", "behavior"), "preferences", true);
            UserSleepSchedule = IOManager.LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior"), "sleep_schedule", true);
        }
    }
}
