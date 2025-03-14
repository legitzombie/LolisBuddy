using LinePutScript;
using LinePutScript.Converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace VPet.Plugin.LolisBuddy
{
    public class DialogueManager
    {
        private static readonly string TextFolderPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            "text"
        );

        private List<DialogueEntry> dialogues = new List<DialogueEntry>();

        public DialogueManager()
        {
            LoadDialogues();
        }

        /// <summary>
        /// Loads all dialogues from .lps files in the "text" folder
        /// </summary>
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




        /// <summary>
        /// Selects a random dialogue entry based on Type, Name, and Mood
        /// </summary>
        public DialogueEntry GetRandomDialogue(string type, string name, string mood)
        {
            var filteredDialogues = new List<DialogueEntry>();

            if (type.ToLower() != "default")
            {
                filteredDialogues = dialogues.Where(d => d.Type.ToLower() == type.ToLower() && d.Name.ToLower() == name.ToLower() && d.Mood.ToLower() == mood.ToLower()).ToList();
            }
            else
            {
                filteredDialogues = dialogues.Where(d => d.Type.ToLower() == type.ToLower() && d.Mood.ToLower() == mood.ToLower()).ToList();
            }

            if (filteredDialogues.Count > 0)
            {
                Random random = new Random();
                return filteredDialogues[random.Next(filteredDialogues.Count)];
            }
            return null; // No matching dialogue found
        }
    }

    public class DialogueEntry
    {
        [Line] 
        public string Type { get; set; }
        [Line]
        public string Name { get; set; }
        [Line] 
        public string Mood { get; set; }
        [Line] 
        public string Dialogue { get; set; }
        [Line]
        public string SoundEffect { get; set; }
    }
}
