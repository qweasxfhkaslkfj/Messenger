using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace TOP_Messanger
{
    public partial class MainWindow : Window
    {
        private ChatClient chatClient;
        private ObservableCollection<string> messages;

        public MainWindow()
        {
            InitializeComponent();

            if (!System.IO.File.Exists("Messenger.db"))
            {
                DataBase.CreateDB();
                DataBase.StartUserTable();
            }

            messages = new ObservableCollection<string>();
            listBoxChat.ItemsSource = messages;

            chatClient = new ChatClient(this);

            if (!Registration.IsServer)
            {
                if (!chatClient.Connect())
                    MessageBox.Show("Не удалось подключиться к серверу", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                messages.Add("[Система] Вы запустили сервер. Ожидаем подключений...");
            }

        }

        /// <summary>
        /// Процедура добавления сообщений в чат
        /// </summary>
        public void AddMessage(string message)
        {
            try
            {
                if (message.StartsWith("LOGIN:"))
                {
                    string[] parts = message.Split(':');
                    if (parts.Length >= 2)
                    {
                        string login = parts[1];
                        string role;

                        if (parts.Length >= 3)
                            role = parts[2];
                        else
                            role = "User";

                        messages.Add($"[Система] {login} ({role}) подключился к чату");
                    }
                    return;
                }

                messages.Add(message);

                if (listBoxChat.Items.Count > 0)
                    listBoxChat.ScrollIntoView(listBoxChat.Items[listBoxChat.Items.Count - 1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления сообщения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Процедуры отправки сообщений
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }
        private void SendMessage()
        {
            string messageText = MessageTextBox.Text.Trim();

            if (string.IsNullOrEmpty(messageText))
                return;

            if (Registration.IsServer)
            {
                string censoredMessage = Censor.Censoring(messageText);
                string fullMessage = $"[{DateTime.Now:HH:mm}] {Registration.userLogin} (сервер): {censoredMessage}";

                ChatServer.Instance.EnqueueMessage(fullMessage);
                messages.Add(fullMessage);
            }
            else
            {
                if (!chatClient.IsConnected)
                {
                    MessageBox.Show("Нет подключения к серверу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    string login = Registration.userLogin;

                    chatClient.SendMessage(messageText);

                    string censoredMessage = Censor.Censoring(messageText);
                    string localMessage = $"[{DateTime.Now:HH:mm}] {login}: {censoredMessage}";
                    messages.Add(localMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка отправки: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MessageTextBox.Text = "";

            if (listBoxChat.Items.Count > 0)
            {
                listBoxChat.ScrollIntoView(listBoxChat.Items[listBoxChat.Items.Count - 1]);
            }
        }

        /// <summary>
        /// Процедура копирования сообщений в буфер обмена
        /// </summary>
        private void ListBoxChat_SelectedItem(object sender, EventArgs e)
        {
            if (listBoxChat.SelectedItem != null)
            {
                string selectedText = listBoxChat.SelectedItem.ToString();
                Clipboard.SetText(selectedText);
            }
        }
        /// <summary>
        /// Обработчики событий нажатий на кнопки
        /// </summary>
        // Кнопка настроек
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }
        // Кнопка игры
        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция игры находится в разработке", "В разработке", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Кнопка отправки файла
        private void SendFileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция отправки файла находится в разработке", "В разработке", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Обработчики закрытия окна
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (Registration.IsServer)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?\nЭто остановит сервер для всех пользователей!", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }
        // Закрытие окна
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (Registration.IsServer && Registration.IsServerRunning)
                Registration.StopServer();

            if (!Registration.IsServer && chatClient != null && chatClient.IsConnected)
                chatClient.Disconnect();
        }
    }
}