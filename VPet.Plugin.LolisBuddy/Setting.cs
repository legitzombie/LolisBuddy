using LinePutScript;
using LinePutScript.Converter;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows;

namespace VPet.Plugin.LolisBuddy
{
    public class Setting
    {

        private static readonly string ConfigPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            "config.lps"
        );

        private int delayTimer = 60000; // loop interval
        private int delayTalk = 5000; // minimum delay between dialogue
        private int chanceTalk = 5; // chance to talk
        private bool debug = false; // debugging
        private bool soundeffect = false; // animation sound effects


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
        public void LoadSettings()
        {
            if (File.Exists(ConfigPath))
            {
                try
                {
                    LpsDocument lps = new LpsDocument(File.ReadAllText(ConfigPath));
                    var loadedSettings = LPSConvert.DeserializeObject<Setting>(lps); 
                    if (loadedSettings != null)
                    {
                        // Copy loaded values to this instance
                        DelayTimer = loadedSettings.DelayTimer;
                        DelayTalk = loadedSettings.DelayTalk;
                        ChanceTalk = loadedSettings.ChanceTalk;
                        Debug = loadedSettings.Debug;
                        SoundEffect = loadedSettings.SoundEffect;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load settings: " + ex.Message);
                    // If file is corrupted, fall back to default values
                }
            }
        }

        /// <summary>
        /// Saves current settings to config.lps
        /// </summary>
        public void SaveSettings()
        {
            try
            {
                LpsDocument lps = LPSConvert.SerializeObject(this);
                File.WriteAllText(ConfigPath, lps.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save settings: " + ex.Message);
            }
        }
    }
}
