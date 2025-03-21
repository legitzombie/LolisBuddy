using System.ComponentModel;
using System.Printing.IndexedProperties;
using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.Config
{
    public class AISetting
    {

        private int delayTimer = 60000; // loop interval
        private int delayTalk = 5000; // minimum delay between dialogue
        private int chanceTalk = 5; // chance to talk

        private int learningSpeed = 1;
        private int learningRate = 5000;
        private int baseDecayRate = 1; // Slower decay
        private int overindulgencePenalty = 2000; // More boredom effect
        private int cravingBoost = 3000; // Stronger craving effect
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

        [Line]
        public int LearningSpeed
        {
            get => learningSpeed;
            set { learningSpeed = value; OnPropertyChanged(nameof(learningSpeed)); }
        }

        [Line]
        public int LearningRate
        {
            get => learningRate;
            set { learningRate = value; OnPropertyChanged(nameof(learningRate)); }
        }

        [Line]
        public int BaseDecayRate
        {
            get => baseDecayRate;
            set { baseDecayRate = value; OnPropertyChanged(nameof(baseDecayRate)); }
        }

        [Line]
        public int OverindulgencePenalty
        {
            get => overindulgencePenalty;
            set { overindulgencePenalty = value; OnPropertyChanged(nameof(overindulgencePenalty)); }
        }

        [Line]
        public int CravingBoost
        {
            get => cravingBoost;
            set { cravingBoost = value; OnPropertyChanged(nameof(cravingBoost)); }
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
            LearningSpeed = loadedSettings.LearningSpeed;
            LearningRate = loadedSettings.LearningRate;
            BaseDecayRate = loadedSettings.BaseDecayRate;
            OverindulgencePenalty = loadedSettings.OverindulgencePenalty;
            CravingBoost = loadedSettings.CravingBoost;
        }

    }
}
