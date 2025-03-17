using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPet.Plugin.LolisBuddy
{
    public class AIManager
    {
        private readonly Setting AIsetting = new Setting();
        public AIManager() { AIsetting.Name = "AIspeech";  AIsetting.DelayTalk = 20000; AIsetting.ChanceTalk = 2; AIsetting.DelayTimer = 60000; }
        
        public Setting settings()
        {
            return AIsetting;
        }
    }
}
