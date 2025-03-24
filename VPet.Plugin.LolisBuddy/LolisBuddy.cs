using System.Windows;
using System.Windows.Controls;
using LinePutScript.Localization.WPF;
using VPet.Plugin.LolisBuddy.Config;
using VPet.Plugin.LolisBuddy.Core;
using VPet.Plugin.LolisBuddy.UI;
using VPet.Plugin.LolisBuddy.Utilities;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy
{
    public class LolisBuddy : MainPlugin
    {
        private TimerManager AITalkTimer;
        private TimerManager PersonalityTimer;

        private readonly TimerManager idleTimer = new TimerManager("idle", 10000, 100);
        private readonly TimerManager windowTimer = new TimerManager("activewindowupdater", 1000, 100);

        public static AISetting AIsetting = new AISetting();

        public LolisBuddy(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {

            AIsetting.Load();

            EventsManager.assignEvents(MW);

            AIManager.Instance.updateMemory();
            AITalkTimer = new TimerManager(AIsetting.Name, AIsetting.DelayTimer, AIsetting.ChanceTalk);
            PersonalityTimer = new TimerManager("AIpersonality", 6000, 100);
            InitializeTimers();
            AddSettingsMenu();
        }

        private void InitializeTimers()
        {
            AITalkTimer.AddOrUpdateTimer(() =>
                 DialogueManager.HandleDialogue(MW, AITalkTimer));

            windowTimer.AddOrUpdateTimer(() => WindowManager.UpdateActiveWindowDetails());

            idleTimer.AddOrUpdateTimer(() => SleepTracker.CheckUserActivity(MW));

            PersonalityTimer.AddOrUpdateTimer(() => PreferenceManager.Instance.Update());
        }

        private void AddSettingsMenu()
        {
            var menuItem = new MenuItem
            {
                Header = "LolisBuddy".Translate(),
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            menuItem.Click += (s, e) => UIManager.Display();
            MW.Main.ToolBar.MenuMODConfig.Items.Add(menuItem);
        }

        public override string PluginName => "LolisBuddy";
    }
}
