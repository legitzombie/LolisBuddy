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
        private TimerManager GameTalkTimer;
        private TimerManager AITalkTimer;

        private TimerManager personalityTimer = new TimerManager("AIpersonality", 6000, 100);
        private readonly TimerManager idleTimer = new TimerManager("idle", 10000, 100);
        private readonly TimerManager windowTimer = new TimerManager("activewindowupdater", 1000, 100);

        public static Setting setting = new Setting();
        public static Setting AIsetting = new Setting();

        public LolisBuddy(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {
            AIsetting.Name = "AIspeech";

            setting.Load();
            AIsetting.Load();

            Actions.assignEvents(MW);

            AIManager.Instance.updateMemory();
            GameTalkTimer = new TimerManager(setting.Name, setting.DelayTimer, setting.ChanceTalk);
            AITalkTimer = new TimerManager(AIsetting.Name, AIsetting.DelayTimer, AIsetting.ChanceTalk);
            InitializeTimers();
            AddSettingsMenu();
        }

        private void InitializeTimers()
        {
            GameTalkTimer.AddOrUpdateTimer(() =>
                DialogueManager.HandleDialogue(MW, GameTalkTimer));

            AITalkTimer.AddOrUpdateTimer(() =>
                 DialogueManager.HandleDialogue(MW, AITalkTimer));

            windowTimer.AddOrUpdateTimer(() => WindowManager.UpdateActiveWindowDetails());

            idleTimer.AddOrUpdateTimer(() => SleepTracker.CheckUserActivity(MW));

            personalityTimer.AddOrUpdateTimer(() => PreferenceManager.Instance.Update());
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
