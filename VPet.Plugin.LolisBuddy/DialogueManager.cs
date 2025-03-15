using LinePutScript;
using LinePutScript.Converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Windows;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy
{
    public class DialogueManager
    {
        private static readonly string TextFolderPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            @"text\"
        );

        private static readonly string SoundFolderPath = Path.Combine(
             Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            @"sound\"
        );

        private static readonly string ConfigFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


        private List<DialogueEntry> dialogues = new List<DialogueEntry>();
        private IOManager iOManager = new IOManager();
        public DialogueEntry dialogue { get; private set; }
        private Random random = new Random();

        private WindowManager windowManager = new WindowManager();

        public DialogueManager()
        {
            LoadDialogues();
        }

        /// Loads all dialogues from .lps files in the "text" folder
        public void LoadDialogues()
        {
            dialogues = iOManager.LoadLPS<DialogueEntry>(TextFolderPath);
        }

        public void playEffect(bool play)
        {
            if (!play) return;

            SoundPlayer player = new SoundPlayer(Path.Combine(SoundFolderPath, dialogue.SoundEffect));
            player.Play();
        }

        public void Talk(IMainWindow main, string msg = null)
        {
            windowManager.UpdateActiveWindowDetails();
            iOManager.SaveLPS(windowManager.Windows, ConfigFolderPath, "memory");
            main.Main.Say(msg ?? dialogue?.Dialogue ?? string.Empty);
        }

        public bool canTalk(int timerElapsed, Setting setting)
        {
            return timerElapsed > setting.DelayTalk && random.Next(100) < setting.ChanceTalk;
        }


        /// <summary>
        /// Selects a random dialogue entry based on Type, Name, and Mood
        /// </summary>
        public DialogueEntry GetRandomDialogue(GraphInfo animation)
        {

            var type = animation.Type.ToString();
            var name = animation.Name;
            var mood = animation.ModeType.ToString();

            var filteredDialogues = dialogues.Where(d =>
                string.Equals(d.Type, type, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.Mood, mood, StringComparison.OrdinalIgnoreCase) &&
                (type.Equals("default", StringComparison.OrdinalIgnoreCase) || string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            if (filteredDialogues.Count == 0)
                return null;

            dialogue = filteredDialogues[random.Next(filteredDialogues.Count)];
            return dialogue;
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
