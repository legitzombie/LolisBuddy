using VPet_Simulator.Windows.Interface;
using System.Windows.Controls;
using System.Windows;
using LinePutScript.Localization.WPF;

namespace VPet.Plugin.LolisBuddy
{
    public class LolisBuddy : MainPlugin
    {
        private readonly TimerManager talkTimer = new TimerManager();
        private readonly TimerManager AItalkTimer = new TimerManager();
        private readonly Setting setting = new Setting();
        private DialogueManager dialogueManager;
        private readonly AIManager aiManager = new AIManager();
        private DialogueManager AIdialogueManager;


        public LolisBuddy(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {
            dialogueManager = new DialogueManager(setting.Name);
            AIdialogueManager = new DialogueManager(aiManager.settings().Name);
            setting.Load();
            InitializeTimer();
            AddSettingsMenu();
        }

        private void InitializeTimer()
        {
            talkTimer.AddOrUpdateTimer(setting.Name, setting.DelayTimer, () => dialogueManager.HandleDialogue(setting, MW, talkTimer));
            AItalkTimer.AddOrUpdateTimer(setting.Name, aiManager.settings().DelayTimer, () => AIdialogueManager.HandleDialogue(aiManager.settings(), MW, AItalkTimer));
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

