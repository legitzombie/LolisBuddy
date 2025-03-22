using System.ComponentModel;
using System.Printing.IndexedProperties;
using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.Config
{
    public class AISetting
    {

        private static AISetting _instance;
        private static readonly object _lock = new object();

        public static AISetting Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AISetting();
                    return _instance;
                }
            }
        }

        private int delayTimer = 60000; // loop interval
        private int delayTalk = 5000; // minimum delay between dialogue
        private int chanceTalk = 5; // chance to talk
        public string Name { set; get; } = "AIspeech";

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void Load()
        {
            AISetting loadedSettings = IOManager.LoadLPS<AISetting>(FolderPath.Get(), "AIconfig")[0];
            Set(loadedSettings);
        }

        public void Save()
        {

            IOManager.SaveLPS(LolisBuddy.AIsetting, FolderPath.Get(), "AIconfig", false, false);
            LolisBuddy.AIsetting.Load();

        }
        private void Set(AISetting loadedSettings)
        {
            DelayTimer = loadedSettings.DelayTimer;
            DelayTalk = loadedSettings.DelayTalk;
            ChanceTalk = loadedSettings.ChanceTalk;
        }

    }
}
