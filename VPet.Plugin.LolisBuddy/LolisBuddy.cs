using VPet_Simulator.Windows.Interface;
using System.Windows.Controls;
using System.Windows;
using LinePutScript.Localization.WPF;
using System.Reflection;
using System.IO;

namespace VPet.Plugin.LolisBuddy
{
    public class LolisBuddy : MainPlugin
    {
        private readonly TimerManager talkTimer = new TimerManager();
        private readonly TimerManager AItalkTimer = new TimerManager();
        private readonly TimerManager idleTimer = new TimerManager();
        private readonly SleepTracker sleepTracker = new SleepTracker();
        private readonly Setting setting = new Setting();
        private DialogueManager dialogueManager;
        private readonly AIManager aiManager = new AIManager();
        private DialogueManager AIdialogueManager;

        private static readonly string DialogueFolderPath = Path.Combine(
    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
    @"text\"
);


        public LolisBuddy(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {
            dialogueManager = new DialogueManager(null, DialogueFolderPath);
            AIdialogueManager = new DialogueManager();
            setting.Load();
            InitializeTimer();
            AddSettingsMenu();
        }

        private void InitializeTimer()
        {
            talkTimer.AddOrUpdateTimer(setting.Name, setting.DelayTimer, () => dialogueManager.HandleDialogue(setting, MW, talkTimer));
            AItalkTimer.AddOrUpdateTimer(aiManager.setting.Name, aiManager.setting.DelayTimer, () => AIdialogueManager.HandleDialogue(aiManager.setting, MW, AItalkTimer, aiManager));
            idleTimer.AddOrUpdateTimer("idle", 60000, () => sleepTracker.CheckUserActivity(MW));
        }

        private void AddSettingsMenu()
        {
            var menuItem = new MenuItem
            {
                Header = "LolisBuddy".Translate(),
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            menuItem.Click += (s, e) => setting.Display();
            MW.Main.ToolBar.MenuMODConfig.Items.Add(menuItem);
        }


        public override string PluginName => "LolisBuddy";
    }
}

