using LinePutScript;
using LinePutScript.Converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy
{
    public class DialogueManager
    {
        private static readonly string TextFolderPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            "text"
        );

        private static readonly string SoundFolderPath = Path.Combine(
             Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            "sound"
        );

        private List<DialogueEntry> dialogues = new List<DialogueEntry>();
        public DialogueEntry dialogue { get; private set; }
        private Random random = new Random();

        public DialogueManager()
        {
            LoadDialogues();
        }

        /// Loads all dialogues from .lps files in the "text" folder
        public void LoadDialogues()
        {
            DirectoryInfo di = new DirectoryInfo(TextFolderPath);

            foreach (FileInfo fi in di.EnumerateFiles("*.lps"))
            {
                var tmp = new LpsDocument(File.ReadAllText(fi.FullName));
                foreach (ILine li in tmp)
                {
                    dialogues.Add(LPSConvert.DeserializeObject<DialogueEntry>(li));
                }
            }
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


        /// <summary>
        /// Selects a random dialogue entry based on Type, Name, and Mood
        /// </summary>
        public DialogueEntry GetRandomDialogue(GraphInfo animation)
        {
            if (animation == null) return null;

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
