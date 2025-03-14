using VPet_Simulator.Windows.Interface;
using System.Timers;
using Timer = System.Timers.Timer;
using LinePutScript.Localization.WPF;
using System.Windows.Controls;
using System.Windows;
using System;
using VPet_Simulator.Core;
using System.Media;
using System.Reflection;
using System.IO;


/* Goal
 * is not sleeping or talking >
 * if passes a RNG roll >
 * if AnimationName and VPetMood matches a dialogue category >
 * Say a random dialogue within all parameters specified.
 */

namespace VPet.Plugin.LolisBuddy
{
    public class LolisBuddy : MainPlugin
    {

        private static readonly string SoundFolderPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            "sound"
        );

        static public Timer talkTimer = new Timer();
        public Random random = new Random();

        public GraphInfo animation = new GraphInfo();

        public DialogueManager dialogueManager = new DialogueManager();

        public DialogueEntry dialogue = new DialogueEntry();

        public Setting setting = new Setting();

        public int timerElapsed = 0; // time since last anim

        public LolisBuddy(IMainWindow mainwin) : base(mainwin)
        {
            
        }
        public override void LoadPlugin()
        {
            setting.LoadSettings();
            talkTimer.Interval = setting.DelayTimer;
            talkTimer.Elapsed += talkTimer_Elapsed;
            talkTimer.AutoReset = true;
            talkTimer.Start();

            var menuItem = new MenuItem()
            {
                Header = "LolisBuddy".Translate(),
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            menuItem.Click += (s, e) => { Setting(); };
            MW.Main.ToolBar.MenuMODConfig.Items.Add(menuItem);
        }

        winSetting winSetting;
        public override void Setting()
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

        private void talkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            timerElapsed += setting.DelayTimer;
            setting.LoadSettings();
            talkTimer.Interval = setting.DelayTimer;


            animation = MW.Main.DisplayType;
            if (canTalk())
            {
                TriggerEvent();
                timerElapsed = 0;
            }
        }

        private bool canTalk()
        {
            bool isAvailable = false;
            if (timerElapsed > setting.DelayTalk)
            {
                if (random.Next(100) < setting.ChanceTalk)
                {
                    isAvailable = true;
                }
            }
            return isAvailable;
        }

        private void getDialogue()
        {
            dialogue = dialogueManager.GetRandomDialogue(animation.Type.ToString(),animation.Name, animation.ModeType.ToString());
        }

        private void TriggerEvent()
        {
            if (setting.Debug)
            {
                MW.Main.Say(debugMessage());
            }
            else
            {

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    getDialogue();
                    MW.Main.Say(dialogue.Dialogue);
                    if (setting.SoundEffect) playEffect();
                });
            }
        }

        public void playEffect()
        {
            SoundPlayer player = new SoundPlayer(Path.Combine(SoundFolderPath, dialogue.SoundEffect));
            player.Play();
        }

        private string debugMessage()
        {
            return $"Animation Type: {animation.Type} \n Animation Name: {animation.Name} \n Animation Mood: {animation.ModeType} \n Animation Animat: {animation.Animat}";
        }

        public override string PluginName => "LolisBuddy";
    }
}
