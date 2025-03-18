using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public static readonly string ActionMemoryPath = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
@"memory\actions\"
);

        IOManager iOManager = new IOManager();
        public List<DialogueEntry> SpeechMemory = new List<DialogueEntry>();
        public List<ProgramBehavior> ProgramBehaviorsMemory = new List<ProgramBehavior>();
        public List<IdleEntry> IdleMemory = new List<IdleEntry>();

        public readonly Setting setting = new Setting();
        public AIManager() 
        { 
            setting.Name = "AIspeech";
            setting.DelayTalk = 20000;
            setting.ChanceTalk = 3;
            setting.DelayTimer = 30000;

            SpeechMemory = iOManager.LoadLPS<DialogueEntry>(SpeechMemoryFolderPath, null, true);
            ProgramBehaviorsMemory = iOManager.LoadLPS<ProgramBehavior>(BehaviorMemoryFolderPath, "user_likes", true);
            IdleMemory = iOManager.LoadLPS<IdleEntry>(BehaviorMemoryFolderPath, "user_schedule", true);
        }
        

    }
}
