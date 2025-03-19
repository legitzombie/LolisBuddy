using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using LinePutScript;
using LinePutScript.Converter;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy
{
    public class DialogueManager
    {

        private static readonly string SoundFolderPath = Path.Combine(
             Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            @"sound\"
        );

        private List<DialogueEntry> dialogues = new List<DialogueEntry>();

        private IOManager iOManager = new IOManager();
        private AIManager aIManager = new AIManager();
        public DialogueEntry dialogue { get; private set; }
        private Random random = new Random();
        private TimerManager timerManager = new TimerManager();
        private WindowManager windowManager = new WindowManager();
        private AnimationManager animationManager = new AnimationManager();
        private int lastDialogue = 0; // how long since last dialogue

        public DialogueManager(string name = null, string path = null, bool encrypted = false, bool erase = false)
        {
            LoadDialogues(name, path, encrypted, erase);
            timerManager.AddOrUpdateTimer("activewindowupdater", 1000, () => windowManager.UpdateActiveWindowDetails());
        }

        /// Loads all dialogues from .lps files in the "text" folder
        public void LoadDialogues(string name, string path, bool encrypted, bool erase = false)
        {
            dialogues = iOManager.LoadLPS<DialogueEntry>(path, name, encrypted, erase);
        }

        public void playEffect(bool play)
        {
            if (!play) return;

            SoundPlayer player = new SoundPlayer(Path.Combine(SoundFolderPath, dialogue.SoundEffect));
            player.Play();
        }

        public void Talk(IMainWindow main, string msg = null)
        {
            main.Main.Say(msg ?? dialogue?.Dialogue ?? string.Empty);
        }

        public bool canTalk(int timerElapsed, Setting setting)
        {
            return timerElapsed > setting.DelayTalk && random.Next(100) < setting.ChanceTalk;
        }

        public void PlayDialogue(Setting setting, IMainWindow MW, AIManager aIManager = null)
        {
            if (setting.Debug && setting.Name == "speech")
            {
                Talk(MW, animationManager.debugMessage());
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    GetRandomDialogue(setting.Name, animationManager.animation, aIManager);
                    Talk(MW);
                    playEffect(setting.SoundEffect);
                });
            }
        }

        public void HandleDialogue(Setting setting, IMainWindow MW, TimerManager talkTimer, AIManager aIManager = null)
        {

            lastDialogue += setting.DelayTimer;
            setting.Load();
            talkTimer.UpdateTimerInterval(setting.Name, setting.DelayTimer);

            animationManager.fetchAnimation(MW);

            if (canTalk(lastDialogue, setting))
            {
                PlayDialogue(setting, MW, aIManager);
            }
        }


        /// <summary>
        /// Selects a random dialogue entry based on Type, Name, and Mood
        /// </summary>
        public void GetRandomDialogue(string Name, GraphInfo animation, AIManager AIManager = null)
        {

            var type = animation.Type.ToString();
            var name = animation.Name;
            var mood = animation.ModeType.ToString();
            List<DialogueEntry> filteredDialogues = new List<DialogueEntry>();

            // short term memory
            if (Name == "speech")
            {
                aIManager.updateMemory();
                filteredDialogues = dialogues.Where(d =>
                string.Equals(d.Type, type, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.Mood, mood, StringComparison.OrdinalIgnoreCase) &&
                (type.Equals("default", StringComparison.OrdinalIgnoreCase) || string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase))
                ).ToList();
                if (filteredDialogues.Count == 0) { return; }
                dialogue = filteredDialogues[random.Next(filteredDialogues.Count)];

                List<DialogueEntry> allreplies = aIManager.ShortSpeechMemory;
                allreplies.Add(dialogue);
                iOManager.SaveLPS(allreplies, AIManager.MemoryTypePath(AIManager.SpeechMemoryFolderPath, true), null, true, true);
            }
            // long term memory
            if (Name == "AIspeech")
            {
                AIManager.updateMemory();
                dialogue = LanguageManager.GenerateSentence(mood, windowManager.window.Category.ToString());
                List<DialogueEntry> allreplies = AIManager.SpeechMemory;
                allreplies.Add(dialogue);
                iOManager.SaveLPS(allreplies, AIManager.MemoryTypePath(AIManager.SpeechMemoryFolderPath, false), null, true, true);
            }

            if (dialogue.Dialogue.Length > 0) lastDialogue = 0;
        }

    }

    public class DialogueEntry
    {
        [Line] public string Type { get; set; }
        [Line] public string Name { get; set; }
        [Line] public string Mood { get; set; }
        [Line] public string Dialogue { get; set; }
        [Line] public string SoundEffect { get; set; }
    }
}
