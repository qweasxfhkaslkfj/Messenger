using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TOP_Messanger
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Интерфейс отправки сообщений
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }
        // ВРЕМЕННАЯ отправка сообщений
        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(MessageTextBox.Text.Trim()))
            {
                string message = Censor.Censoring(MessageTextBox.Text);
                listBoxChat.Items.Add(message);
                MessageTextBox.Clear();
            }
        }

        // Копирование сообщений в буфер обмена
        private void ListBoxChat_SelectedItem(object sender, EventArgs e)
        {
            if (listBoxChat.SelectedItem != null)
            {
                string selectedText = listBoxChat.SelectedItem.ToString();
                Clipboard.SetText(selectedText);
            }
        }

        // Кнопка настроек
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
        // Кнопка игры
        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция игры находится в разработке");
        }
        // Кнопка отправки файла
        private void SendFileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция отправки файла находится в разработке");
        }
    }
}
