using System.ComponentModel;
using System.IO;
using System.Reflection;
using LinePutScript;
using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.Config
{
    public class Setting
    {

        private int delayTimer = 60000; // loop interval
        private int delayTalk = 5000; // minimum delay between dialogue
        private int chanceTalk = 5; // chance to talk
        private bool debug = false; // debugging
        private bool soundeffect = false; // animation sound effects
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
            if (Name == "speech")
            {
                Setting loadedSettings = IOManager.LoadLPS<Setting>(FolderPath.Get(), "config")[0];
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

    }
}

