using System.Windows;

namespace VPet.Plugin.LolisBuddy
{
    /// <summary>
    /// Interaction logic for winSetting.xaml
    /// </summary>
    public partial class winSetting : Window
    {
        public Setting UserSettings { get; set; }

        public winSetting()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UserSettings = new Setting(); // Load or create settings instance
            UserSettings.LoadSettings(); // Load settings from file
            DataContext = UserSettings;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UserSettings.SaveSettings(); // Save settings when clicking the button
            Close();
        }
    }
}
