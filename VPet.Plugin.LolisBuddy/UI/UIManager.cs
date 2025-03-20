using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.UI
{
    public static class UIManager
    {
        private static winSetting winSetting;
        public static void Display()
        {
            if (winSetting != null)
            {
                winSetting.Activate();
                return;
            }
            else
            {
                winSetting = new winSetting();
                winSetting.Closed += (s, e) => { winSetting = null; };
                winSetting.Show();
            }
        }


    }
}
