using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VPet.Plugin.LolisBuddy
{
    public class AIManager
    {
        public static readonly string SpeechMemoryFolderPath = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
@"memory\speech\"
);

        public static readonly string BehaviorMemoryFolderPath = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
@"memory\behavior\"
);

        public static readonly string SleepMemoryFolderPath = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
@"memory\behavior\sleep\"
);

        public static readonly string ActionMemoryPath = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
@"memory\actions\"
);

        public static string MemoryTypePath(string path, bool shortTerm = false)
        {
            if (shortTerm) return Path.Combine(path, "short_term");
            return Path.Combine(path, "long_term");
        }



        IOManager iOManager = new IOManager();
        public List<DialogueEntry> SpeechMemory = new List<DialogueEntry>();
        public List<DialogueEntry> ShortSpeechMemory = new List<DialogueEntry>();

        public List<IdleEntry> IdleMemory = new List<IdleEntry>();
        public List<IdleEntry> ShortIdleMemory = new List<IdleEntry>();

        public List<ActiveWindow> ActionMemory = new List<ActiveWindow>();
        public List<ActiveWindow> ShortActionMemory = new List<ActiveWindow>();

        public List<ProgramBehavior> UserPreferences = new List<ProgramBehavior>();
        public List<IdleEntry> UserSleepSchedule = new List<IdleEntry>();

        public readonly Setting setting = new Setting();
        public AIManager()
        {
            setting.Name = "AIspeech";
            setting.DelayTalk = 20000;
            setting.ChanceTalk = 1;
            setting.DelayTimer = 30000;

            updateMemory();
        }

        public void updateMemory()
        {
            SpeechMemory = iOManager.LoadLPS<DialogueEntry>(MemoryTypePath(SpeechMemoryFolderPath, false), null, true, true);
            ShortSpeechMemory = iOManager.LoadLPS<DialogueEntry>(MemoryTypePath(SpeechMemoryFolderPath, true), null, true, true);

            IdleMemory = iOManager.LoadLPS<IdleEntry>(MemoryTypePath(SleepMemoryFolderPath, false), null, true, true);
            ShortIdleMemory = iOManager.LoadLPS<IdleEntry>(MemoryTypePath(SleepMemoryFolderPath, true), null, true, true);

            ActionMemory = iOManager.LoadLPS<ActiveWindow>(MemoryTypePath(ActionMemoryPath, false), null, true, true);
            ShortActionMemory = iOManager.LoadLPS<ActiveWindow>(MemoryTypePath(ActionMemoryPath, false), null, true, true);

            UserPreferences = iOManager.LoadLPS<ProgramBehavior>(BehaviorMemoryFolderPath, "preferences", true);
            UserSleepSchedule = iOManager.LoadLPS<IdleEntry>(BehaviorMemoryFolderPath, "sleep_schedule", true);
        }


    }
}
