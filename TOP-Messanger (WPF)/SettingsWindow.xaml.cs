using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TOP_Messanger
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            userName_TextBox.Text = Registration.userLogin;
        }
        // перемещение по настройкам
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            scroller.ScrollToTop();
        }
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            scroller.ScrollToVerticalOffset(100);
        }
        private void RatingButton_Click(object sender, RoutedEventArgs e)
        {
            scroller.ScrollToBottom();
        }
        // Закрытие настроек
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
