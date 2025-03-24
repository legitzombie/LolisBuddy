using System.Windows;
using VPet.Plugin.LolisBuddy.Config;

namespace VPet.Plugin.LolisBuddy
{
    /// <summary>
    /// Interaction logic for winSetting.xaml
    /// </summary>
    public partial class winSetting : Window
    {
        public AISetting LolisBuddySettings { get; set; }

        public winSetting()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            LolisBuddySettings = LolisBuddy.AIsetting;

            LolisBuddySettings.Load();

            DataContext = LolisBuddySettings;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            LolisBuddySettings.Save();
            Close();
        }
    }
}
