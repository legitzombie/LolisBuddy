using System.Windows;
using VPet.Plugin.LolisBuddy.Config;

namespace VPet.Plugin.LolisBuddy
{
    /// <summary>
    /// Interaction logic for winSetting.xaml
    /// </summary>
    public partial class winSetting : Window
    {
        public Setting UserSettings { get; set; }
        public Setting LolisBuddySettings { get; set; }

        public winSetting()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            UserSettings = LolisBuddy.setting;
            LolisBuddySettings = LolisBuddy.AIsetting;

            UserSettings.Load();
            LolisBuddySettings.Load();

            GameTab.DataContext = UserSettings;
            LolisBuddyTab.DataContext = LolisBuddySettings;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UserSettings.Save();
            LolisBuddySettings.Save();
            Close();
        }
    }
}
