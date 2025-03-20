using System.Collections.Generic;
using System.Windows;
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

        public static List<DialogueEntry> SpeechMemory { get; set; } = new List<DialogueEntry>();
        public static List<DialogueEntry> ShortSpeechMemory { get; set; } = new List<DialogueEntry>();

        public static List<IdleEntry> IdleMemory { get; set; } = new List<IdleEntry>();
        public static List<IdleEntry> ShortIdleMemory { get; set; } = new List<IdleEntry>();
        public static List<IdleEntry> SleepMemory { get; set; } = new List<IdleEntry>();

        public static List<WindowEntry> ProgramMemory { get; set; } = new List<WindowEntry>();
        public static List<WindowEntry> ShortProgramMemory { get; set; } = new List<WindowEntry>();

        public static List<PreferenceEntry> ProgramPreferences { get; set; } = new List<PreferenceEntry>();
        public static List<PreferenceEntry> FoodPreferences { get; set; } = new List<PreferenceEntry>();
        public static List<PreferenceEntry> ActionPreferences { get; set; } = new List<PreferenceEntry>();

        public static List<FoodEntry> FoodMemory { get; internal set; } = new List<FoodEntry>();
        public static List<ActionEntry> ActionMemory { get; internal set; } = new List<ActionEntry>();

        public void updateMemory(string category = null)
        {
            switch(category)
            {
                case "speech":
                    loadSpeech();
                    break;
                case "sleep":
                    loadSleep();
                    break;
                case "idle":
                    loadIdle();
                    break;
                case "actions":
                    loadActions();
                    break;
                case "preferences":
                    loadPreferences();
                    break;
                default:
                    loadSpeech();
                    loadSleep();
                    loadActions();
                    loadPreferences();
                    loadIdle();
                    break;
            }
        }

        public void saveMemory(string category = null)
        {
            switch (category)
            {
                case "speech":
                    saveSpeech();
                    break;
                case "sleep":
                    saveSleep();
                    break;
                case "idle":
                    saveIdle();
                    break;
                case "actions":
                    saveActions();
                    break;
                case "preferences":
                    savePreferences();
                    break;
                default:
                    saveSpeech();
                    saveSleep();
                    saveActions();
                    savePreferences();
                    saveIdle();
                    break;
            }
        }

        private void loadSpeech()
        {
            SpeechMemory = IOManager.LoadLPS<DialogueEntry>(FolderPath.Get("memory", "speech", "long_term"), null, false, true);
            ShortSpeechMemory = IOManager.LoadLPS<DialogueEntry>(FolderPath.Get("memory", "speech", "short_term"), null, false, true);
        }

        private void loadIdle()
        {
            IdleMemory = IOManager.LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior", "idle", "long_term"), null, false, true);
            ShortIdleMemory = IOManager.LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior", "idle", "short_term"), null, false, true);
        }

        private void loadActions()
        {
            ProgramMemory = IOManager.LoadLPS<WindowEntry>(FolderPath.Get("memory", "actions", "long_term"), null, false, true);
            ShortProgramMemory = IOManager.LoadLPS<WindowEntry>(FolderPath.Get("memory", "actions", "short_term"), null, false, true);
            ProgramMemory = WindowManager.RemoveDuplicates(ProgramMemory);
            ShortProgramMemory = WindowManager.RemoveDuplicates(ShortProgramMemory);

        }

        private void loadPreferences()
        {
            ProgramPreferences = IOManager.LoadLPS<PreferenceEntry>(FolderPath.Get("memory", "personality", "preferences"), "apps", false);
         
        }

        private void loadSleep()
        {
            SleepMemory = IOManager.LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior", "sleep"), "schedule", false, false);
        }

        private void saveSleep()
        {
            IOManager.SaveLPS(SleepMemory, FolderPath.Get("memory", "behavior", "sleep"), "schedule", false, false);
        }

        private void saveSpeech()
        {
            IOManager.SaveLPS(SpeechMemory, FolderPath.Get("memory", "speech", "long_term"), null, false, false);
            IOManager.SaveLPS(ShortSpeechMemory, FolderPath.Get("memory", "speech", "short_term"), null, false, false);
        }

        private void saveIdle()
        {
            IOManager.SaveLPS(IdleMemory, FolderPath.Get("memory", "behavior", "idle", "long_term"), null, false, false);
            IOManager.SaveLPS(ShortIdleMemory, FolderPath.Get("memory", "behavior", "idle", "short_term"), null, false, false);
        }

        private void saveActions()
        {
            ProgramMemory = WindowManager.RemoveDuplicates(ProgramMemory);
            ShortProgramMemory = WindowManager.RemoveDuplicates(ShortProgramMemory);
            IOManager.SaveLPS(ProgramMemory, FolderPath.Get("memory", "actions", "long_term"), null, false, false);
            IOManager.SaveLPS(ShortProgramMemory, FolderPath.Get("memory", "actions", "short_term"), null, false, false);
        }

        private void savePreferences()
        {
            IOManager.SaveLPS(ProgramPreferences, FolderPath.Get("memory", "personality", "preferences"), "apps", false);
        }
    }
}
