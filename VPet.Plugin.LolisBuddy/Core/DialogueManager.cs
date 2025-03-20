using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using LinePutScript;
using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.UI;
using VPet.Plugin.LolisBuddy.Utilities;
using VPet_Simulator.Windows.Interface;


namespace VPet.Plugin.LolisBuddy.Core
{
    public class DialogueManager
    {

        private List<DialogueEntry> dialogues = new List<DialogueEntry>();
        public DialogueEntry dialogue { get; private set; }
        private Random random = new Random();
        private int lastDialogue = 0; // how long since last dialogue

        public DialogueManager(string path = null, string name = null, bool encrypted = false, bool erase = false)
        {
            if (path != null) dialogues = IOManager.LoadLPS<DialogueEntry>(path, name, encrypted, erase);
        }

        public void playEffect(bool play)
        {
            if (!play) return;

            SoundPlayer player = new SoundPlayer(Path.Combine(FolderPath.Get("sound"), dialogue.SoundEffect));
            player.Play();
        }

        public void Talk(IMainWindow main, string msg = null)
        {
            main.Main.Say(msg ?? dialogue?.Dialogue ?? string.Empty);
        }

        public bool canTalk(int timerElapsed, TimerManager timer)
        {
            return timerElapsed > timer.interval && random.Next(100) < timer.chance;
        }

        public void PlayDialogue(IMainWindow MW, TimerManager timer)
        {
            AnimationManager.Instance.updateAnimation(MW);
            if (LolisBuddy.setting.Debug) Talk(MW, AnimationManager.Instance.debugMessage());
            else { 
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    GetRandomDialogue(MW, timer);
                    Talk(MW);
                    playEffect(LolisBuddy.setting.SoundEffect);
                });
            }
        }

        public void HandleDialogue(IMainWindow MW, TimerManager timer)
        {
            string name = timer.name;
            int delay = timer.interval;
            int chance = timer.chance;

            lastDialogue += delay;

            if (name == "speech") timer.UpdateTimerInterval(LolisBuddy.setting.Name, LolisBuddy.setting.DelayTimer, LolisBuddy.setting.ChanceTalk);
            else if (name == "AIspeech") timer.UpdateTimerInterval(LolisBuddy.AIsetting.Name, LolisBuddy.AIsetting.DelayTimer, LolisBuddy.AIsetting.ChanceTalk);
            else timer.UpdateTimerInterval(name, delay, chance);

            if (canTalk(lastDialogue, timer)) PlayDialogue(MW, timer);
        }


        /// <summary>
        /// Selects a random dialogue entry based on Type, Name, and Mood
        /// </summary>
        public void GetRandomDialogue(IMainWindow MW, TimerManager timer)
        {
            
            var type = AnimationManager.Instance.animation.Type.ToString();
            var name = AnimationManager.Instance.animation.Name.ToString();
            var mood = AnimationManager.Instance.animation.ModeType.ToString();

            List<DialogueEntry> filteredDialogues = new List<DialogueEntry>();

            AIManager.Instance.updateMemory("speech");

            // short term memory
            if (timer.name == "speech")
            {
                filteredDialogues = dialogues.Where(d =>
                string.Equals(d.Type, type, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.Mood, mood, StringComparison.OrdinalIgnoreCase) &&
                (type.Equals("default", StringComparison.OrdinalIgnoreCase) || string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase))
                ).ToList();
                if (filteredDialogues.Count == 0) { return; }
                dialogue = filteredDialogues[random.Next(filteredDialogues.Count)];
                
            }
            // long term memory
            if (timer.name == "AIspeech")
            {
                dialogue = LanguageManager.GenerateSentence(mood, WindowManager.window.Category.ToString());
                AIManager.SpeechMemory.Add(dialogue);
                AIManager.ShortSpeechMemory.Add(dialogue);
            }

            AIManager.Instance.saveMemory("speech");

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
