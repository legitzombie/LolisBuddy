using System.Collections.Generic;
using System.Linq;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.UI;
using VPet.Plugin.LolisBuddy.Utilities;
using System;
using System.Windows;

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

        public static string Mood { get; set; } = "";

        public static string Subject { get; set; } = "";

        public static bool CanTalk { get; set; } = false;

        public static void resetSpeech()
        {
            Mood = "";
            Subject = "";
            CanTalk = false;
        }


        public static void setSpeech(string mood, string subject)
        {
            //MessageBox.Show($"3. {mood} \n {subject}");
            Mood = mood;
            Subject = subject;
            CanTalk = true;
        }

        public static List<ActionEntry> ActionMemory { get; set; } = new List<ActionEntry>();
        public static List<ActionEntry> ShortActionMemory { get; set; } = new List<ActionEntry>();

        public static List<DialogueEntry> SpeechMemory { get; set; } = new List<DialogueEntry>();
        public static List<DialogueEntry> ShortSpeechMemory { get; set; } = new List<DialogueEntry>();

        public static List<IdleEntry> IdleMemory { get; set; } = new List<IdleEntry>();
        public static List<IdleEntry> ShortIdleMemory { get; set; } = new List<IdleEntry>();
        public static List<IdleEntry> SleepMemory { get; set; } = new List<IdleEntry>();

        public static List<WindowEntry> ProgramMemory { get; set; } = new List<WindowEntry>();
        public static List<WindowEntry> ShortProgramMemory { get; set; } = new List<WindowEntry>();

        public static List<PreferenceEntry> ProgramPreferences { get; set; } = new List<PreferenceEntry>();
        public static List<PreferenceEntry> ItemPreferences { get; set; } = new List<PreferenceEntry>();
        public static List<PreferenceEntry> ActionPreferences { get; set; } = new List<PreferenceEntry>();
        public static List<PreferenceEntry> TouchPreferences { get; set; } = new List<PreferenceEntry>();

        public static List<ItemEntry> ItemMemory { get; set; } = new List<ItemEntry>();
        public static List<ItemEntry> ShortItemMemory { get; set; } = new List<ItemEntry>();

        public static List<TouchEntry> TouchMemory { get; set; } = new List<TouchEntry>();
        public static List<TouchEntry> ShortTouchMemory { get; set; } = new List<TouchEntry>();

        public void updateMemory(string category = null)
        {
            switch (category)
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
                case "programs":
                    loadPrograms();
                    break;
                case "programspreferences":
                    loadProgramsPreferences();
                    break;
                case "itemspreferences":
                    loadItemsPreferences();
                    break;
                case "actionspreferences":
                    loadActionsPreferences();
                    break;
                case "touchpreferences":
                    loadTouchPreferences();
                    break;
                case "interact":
                    loadInteract();
                    break;
                case "items":
                    loadItems();
                    break;
                case "touch":
                    loadTouch();
                    break;
                default:
                    loadSpeech();
                    loadSleep();
                    loadPrograms();
                    loadIdle();
                    loadInteract();
                    loadItems();
                    loadTouch();
                    loadProgramsPreferences();
                    loadItemsPreferences();
                    loadActionsPreferences();
                    loadTouchPreferences();
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
                case "programs":
                    savePrograms();
                    break;
                case "programspreferences":
                    saveProgramsPreferences();
                    break;
                case "itemspreferences":
                    saveItemsPreferences();
                    break;
                case "actionspreferences":
                    saveActionsPreferences();
                    break;
                case "touchpreferences":
                    saveTouchPreferences();
                    break;
                case "interact":
                    saveInteract();
                    break;
                case "items":
                    saveItems();
                    break;
                case "touch":
                    saveTouch();
                    break;
                default:
                    saveSpeech();
                    saveSleep();
                    savePrograms();
                    saveIdle();
                    saveInteract();
                    saveItems();
                    saveTouch();
                    saveProgramsPreferences();
                    saveItemsPreferences();
                    saveActionsPreferences();
                    saveTouchPreferences();
                    break;
            }
        }

        private void saveTouch()
        {
            IOManager.SaveLPS(TouchMemory, FolderPath.Get("memory", "actions", "touch", "long_term"), null, false, false);
            IOManager.SaveLPS(TouchMemory, FolderPath.Get("memory", "actions", "touch", "short_term"), null, false, false);
        }

        private void loadTouch()
        {
            TouchMemory = IOManager.LoadLPS<TouchEntry>(FolderPath.Get("memory", "actions", "touch", "long_term"), null, false, true);
            ShortTouchMemory = IOManager.LoadLPS<TouchEntry>(FolderPath.Get("memory", "actions", "touch", "short_term"), null, false, true);
        }

        private static void loadItems()
        {
            ItemMemory = IOManager.LoadLPS<ItemEntry>(FolderPath.Get("memory", "actions", "items", "long_term"), null, false, true);
            ShortItemMemory = IOManager.LoadLPS<ItemEntry>(FolderPath.Get("memory", "actions", "items", "short_term"), null, false, true);
        }

        private static void loadInteract()
        {
            ActionMemory = IOManager.LoadLPS<ActionEntry>(FolderPath.Get("memory", "actions", "interact", "long_term"), null, false, true);
            ShortActionMemory = IOManager.LoadLPS<ActionEntry>(FolderPath.Get("memory", "actions", "interact", "short_term"), null, false, true);
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

        private void loadPrograms()
        {
            ProgramMemory = IOManager.LoadLPS<WindowEntry>(FolderPath.Get("memory", "actions", "programs", "long_term"), null, false, true);
            ShortProgramMemory = IOManager.LoadLPS<WindowEntry>(FolderPath.Get("memory", "actions", "programs", "short_term"), null, false, true);
            ProgramMemory = WindowManager.RemoveDuplicates(ProgramMemory);
            ShortProgramMemory = WindowManager.RemoveDuplicates(ShortProgramMemory);

        }

        private void loadProgramsPreferences() => ProgramPreferences = IOManager.LoadLPS<PreferenceEntry>(FolderPath.Get("memory", "personality", "preferences"), "apps", false);
        private void loadItemsPreferences() => ItemPreferences = IOManager.LoadLPS<PreferenceEntry>(FolderPath.Get("memory", "personality", "preferences"), "items", false);
        private void loadActionsPreferences() => ActionPreferences = IOManager.LoadLPS<PreferenceEntry>(FolderPath.Get("memory", "personality", "preferences"), "actions", false);
        private void loadTouchPreferences() => TouchPreferences = IOManager.LoadLPS<PreferenceEntry>(FolderPath.Get("memory", "personality", "preferences"), "touch", false);

        private void loadSleep()
        {
            SleepMemory = IOManager.LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior", "sleep"), "schedule", false, false);
        }

        private static void saveItems()
        {
            IOManager.SaveLPS(ItemMemory, FolderPath.Get("memory", "actions", "items", "long_term"), null, false, false);
            IOManager.SaveLPS(ShortItemMemory, FolderPath.Get("memory", "actions", "items", "short_term"), null, false, false);
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

        private void savePrograms()
        {
            ProgramMemory = WindowManager.RemoveDuplicates(ProgramMemory);
            ShortProgramMemory = WindowManager.RemoveDuplicates(ShortProgramMemory);
            IOManager.SaveLPS(ProgramMemory, FolderPath.Get("memory", "actions", "programs", "long_term"), null, false, false);
            IOManager.SaveLPS(ShortProgramMemory, FolderPath.Get("memory", "actions", "programs", "short_term"), null, false, false);
        }

        private static void saveInteract()
        {
            IOManager.SaveLPS(ActionMemory, FolderPath.Get("memory", "actions", "interact", "long_term"), null, false, false);
            IOManager.SaveLPS(ShortActionMemory, FolderPath.Get("memory", "actions", "interact", "short_term"), null, false, false);
        }

        private static void saveProgramsPreferences() => IOManager.SaveLPS(ProgramPreferences, FolderPath.Get("memory", "personality", "preferences"), "apps", false);
        private static void saveItemsPreferences() => IOManager.SaveLPS(ItemPreferences, FolderPath.Get("memory", "personality", "preferences"), "items", false);
        private static void saveActionsPreferences() => IOManager.SaveLPS(ActionPreferences, FolderPath.Get("memory", "personality", "preferences"), "actions", false);
        private static void saveTouchPreferences() => IOManager.SaveLPS(TouchPreferences, FolderPath.Get("memory", "personality", "preferences"), "touch", false);
    }
}
