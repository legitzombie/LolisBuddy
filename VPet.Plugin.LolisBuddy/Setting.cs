using System.ComponentModel;
using System.IO;
using System.Reflection;
using LinePutScript;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy
{
    public class Setting
    {

        private static readonly string ConfigPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


        private int delayTimer = 60000; // loop interval
        private int delayTalk = 5000; // minimum delay between dialogue
        private int chanceTalk = 5; // chance to talk
        private bool debug = false; // debugging
        private bool soundeffect = false; // animation sound effects

        private IOManager iOManager = new IOManager();
        public string Name { set; get; } = "speech";

        [Line]
        public int DelayTimer
        {
            get => delayTimer;
            set { delayTimer = value; OnPropertyChanged(nameof(DelayTimer)); }
        }

        [Line]
        public int DelayTalk
        {
            get => delayTalk;
            set { delayTalk = value; OnPropertyChanged(nameof(DelayTalk)); }
        }

        [Line]
        public int ChanceTalk
        {
            get => chanceTalk;
            set { chanceTalk = value; OnPropertyChanged(nameof(ChanceTalk)); }
        }

        [Line]
        public bool Debug
        {
            get => debug;
            set { debug = value; OnPropertyChanged(nameof(Debug)); }
        }
        [Line]
        public bool SoundEffect
        {
            get => soundeffect;
            set { soundeffect = value; OnPropertyChanged(nameof(SoundEffect)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads settings from config.lps
        /// </summary>
        public void Load()
        {
            if ( Name == "speech")
            {
                Setting loadedSettings = iOManager.LoadLPS<Setting>(ConfigPath, "config")[0];
                Set(loadedSettings);
            }

        }

        private void Set(Setting loadedSettings)
        {
            DelayTimer = loadedSettings.DelayTimer;
            DelayTalk = loadedSettings.DelayTalk;
            ChanceTalk = loadedSettings.ChanceTalk;
            Debug = loadedSettings.Debug;
            SoundEffect = loadedSettings.SoundEffect;
        }

        winSetting winSetting;
        public void Display()
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

        /// <summary>
        /// Saves current settings to config.lps
        /// </summary>
        public void Save()
        {
            iOManager.SaveLPS(this, ConfigPath, "config");
        }
    }
}
