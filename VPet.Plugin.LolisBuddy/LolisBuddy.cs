using VPet_Simulator.Windows.Interface;
using System.Windows.Controls;
using System.Windows;
using LinePutScript.Localization.WPF;

namespace VPet.Plugin.LolisBuddy
{
    public class LolisBuddy : MainPlugin
    {
        private readonly TimerManager talkTimer = new TimerManager();
        private readonly DialogueManager dialogueManager = new DialogueManager();
        private readonly Setting setting = new Setting();

        public LolisBuddy(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {
            setting.Load();
            InitializeTimer();
            AddSettingsMenu();
        }

        private void InitializeTimer()
        {
            talkTimer.AddOrUpdateTimer("speech", setting.DelayTimer, () => dialogueManager.HandleDialogue(setting, MW, talkTimer));
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

