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
        private TimerManager shortTermTalkTimer;
        private readonly TimerManager longTermTalkTimer = new TimerManager("AIspeech", 30000, 3);
        private readonly TimerManager idleTimer = new TimerManager("idle", 60000, 100);
        private readonly TimerManager windowTimer = new TimerManager("activewindowupdater", 1000, 100);

        private readonly SleepTracker sleepTracker = new SleepTracker();

        public static Setting setting = new Setting();

        private DialogueManager shortTermDialogueManager = new DialogueManager(FolderPath.Get("text"));
        private DialogueManager longTermDialogueManager = new DialogueManager();

        public LolisBuddy(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {
            setting.Load();
            AIManager.Instance.updateMemory();
            shortTermTalkTimer = new TimerManager("speech", setting.DelayTimer, setting.ChanceTalk);
            InitializeTimers();
            AddSettingsMenu();
        }

        private void InitializeTimers()
        {
            shortTermTalkTimer.AddOrUpdateTimer(() =>
                shortTermDialogueManager.HandleDialogue(MW, shortTermTalkTimer));

            longTermTalkTimer.AddOrUpdateTimer(() =>
                 longTermDialogueManager.HandleDialogue(MW, longTermTalkTimer));

            windowTimer.AddOrUpdateTimer(() => WindowManager.UpdateActiveWindowDetails());


            idleTimer.AddOrUpdateTimer(() => sleepTracker.CheckUserActivity(MW));
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
