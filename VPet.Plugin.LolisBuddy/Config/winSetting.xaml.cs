using System.Windows;
using VPet.Plugin.LolisBuddy.Config;
using VPet.Plugin.LolisBuddy.UI;

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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UserSettings = LolisBuddy.setting; // Load or create settings instance
            UserSettings.Load(); // Load settings from file
            DataContext = UserSettings;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UIManager.Save(); // Save settings when clicking the button
            Close();
        }
    }
}
