using VPet_Simulator.Windows.Interface;
using System.Timers;
using System.Windows.Controls;
using System.Windows;
using LinePutScript.Localization.WPF;

namespace VPet.Plugin.LolisBuddy
{
    public class LolisBuddy : MainPlugin
    {
        private static readonly Timer talkTimer = new Timer();
        private readonly DialogueManager dialogueManager = new DialogueManager();
        private readonly AnimationManager animationManager = new AnimationManager();
        private readonly Setting setting = new Setting();
        private int timerElapsed = 0; // Time since last animation

        public LolisBuddy(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {
            setting.Load();
            SetupTalkTimer();
            AddMenuItem();
        }

        private void SetupTalkTimer()
        {
            talkTimer.Interval = setting.DelayTimer;
            talkTimer.Elapsed += OnTalkTimerElapsed;
            talkTimer.AutoReset = true;
            talkTimer.Start();
        }

        private void AddMenuItem()
        {
            var menuItem = new MenuItem
            {
                Header = "LolisBuddy".Translate(),
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            menuItem.Click += (s, e) => setting.Display();
            MW.Main.ToolBar.MenuMODConfig.Items.Add(menuItem);
        }

        private void OnTalkTimerElapsed(object sender, ElapsedEventArgs e)
        {
            timerElapsed += setting.DelayTimer;
            setting.Load();
            talkTimer.Interval = setting.DelayTimer;

            animationManager.fetchAnimation(MW);
            if (dialogueManager.canTalk(timerElapsed, setting))
            {
                TriggerEvent();
                timerElapsed = 0;
            }
        }

        private void TriggerEvent()
        {
            switch (setting.Debug)
            {
                case true:
                    dialogueManager.Talk(MW, animationManager.debugMessage());
                    break;
                case false:
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        PlayDialogue();
                    });
                    break;
            }
        }

        private void PlayDialogue()
        {
            dialogueManager.GetRandomDialogue(animationManager.animation);
            dialogueManager.Talk(MW);
            dialogueManager.playEffect(setting.SoundEffect);
        }

        public override string PluginName => "LolisBuddy";
    }
}

