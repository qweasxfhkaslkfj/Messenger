using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            CheckAdminAccess();
            if (Registration.IsServer)
                LoadUsersList();
        }

        /// <summary>
        /// Процедура проверки админских прав
        /// </summary>
        private void CheckAdminAccess()
        {
            if (Registration.IsServer)
            {
                AdminSection.Visibility = Visibility.Visible;

                Button adminButton = new Button
                {
                    Content = "🛡 Администратор",
                    Style = (Style)FindResource("MenuButtonStyle"),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Height = 45,
                    FontSize = 15,
                    Margin = new Thickness(5, 5, 5, 0)
                };
                adminButton.Click += AdminButton_Click;

                StackPanel stackPanel = (StackPanel)this.FindName("stackPanel");
                if (stackPanel != null)
                    stackPanel.Children.Insert(stackPanel.Children.Count - 1, adminButton);
            }
        }
        /// <summary>
        /// Процедура инициализирующая списки пользователей
        /// </summary>
        private void LoadUsersList()
        {
            try
            {
                UsersListPanel.Children.Clear();

                var users = GetUsersFromDatabase();

                if (users.Count == 0)
                {
                    UsersListPanel.Children.Add(new TextBlock
                    {
                        Text = "Пользователи не найдены",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = Brushes.Gray,
                        FontStyle = FontStyles.Italic
                    });
                    return;
                }

                foreach (var user in users)
                {
                    StackPanel userPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    userPanel.Children.Add(new TextBlock
                    {
                        Text = $"👤 {user.Login}",
                        FontWeight = FontWeights.SemiBold,
                        Foreground = Brushes.DarkSlateGray,
                        Width = 120
                    });

                    userPanel.Children.Add(new TextBlock
                    {
                        Text = $"Роль: {user.Role}",
                        Foreground = Brushes.Gray,
                        Margin = new Thickness(10, 0, 0, 0),
                        Width = 100
                    });

                    userPanel.Children.Add(new TextBlock
                    {
                        Text = $"Рейтинг: {user.Rating}",
                        Foreground = Brushes.DarkBlue,
                        Margin = new Thickness(10, 0, 0, 0)
                    });

                    UsersListPanel.Children.Add(userPanel);
                }
            }
            catch (Exception ex)
            {
                UsersListPanel.Children.Clear();
                UsersListPanel.Children.Add(new TextBlock
                {
                    Text = $"Ошибка загрузки: {ex.Message}",
                    Foreground = Brushes.Red
                });
            }
        }
        /// <summary>
        /// Метод получения пользователей из БД
        /// </summary>
        private List<UserInfo> GetUsersFromDatabase()
        {
            var users = new List<UserInfo>();

            try
            {
                SqliteConnection conn = new SqliteConnection("Data Source=Messenger.db");
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT userName, userRole, userRating FROM user ORDER BY userRole, userName";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserInfo
                        {
                            Login = reader["userName"].ToString(),
                            Role = reader["userRole"].ToString(),
                            Rating = Convert.ToInt32(reader["userRating"])
                        });
                    }
                }
            }
            catch (Exception)
            {
                users.Add(new UserInfo { Login = "server", Role = "server", Rating = 0 });
                users.Add(new UserInfo { Login = "krs333", Role = "user", Rating = 0 });
                users.Add(new UserInfo { Login = "Pagan821", Role = "user", Rating = 0 });
            }

            return users;
        }
        /// <summary>
        /// Обработчики событий нажатий на кнопки
        /// </summary>
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AdminUserDialog("add");
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    DataBase.InsertUserTable(
                        dialog.UserName,
                        dialog.Password,
                        dialog.Role,
                        "#808080",
                        "192.168.88.0"
                    );

                    MessageBox.Show($"Пользователь {dialog.UserName} добавлен", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadUsersList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Удаление пользователей
        private void RemoveUserButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AdminUserDialog("remove");
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string userName = dialog.UserName;

                    int userId = GetUserIdByName(userName);

                    if (userId > 0)
                    {
                        DataBase.DeleteFromUserTable(userId);
                        MessageBox.Show($"Пользователь {userName} удалён", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Пользователь {userName} не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    LoadUsersList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Редактирование пользователей
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AdminUserDialog("edit");
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string userName = dialog.UserName;
                    string password = dialog.Password;
                    string role = dialog.Role;

                    int userId = GetUserIdByName(userName);

                    if (userId > 0)
                    {
                        UpdateUserInDatabase(userId, password, role);

                        MessageBox.Show($"Данные пользователя {userName} обновлены", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Пользователь {userName} не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    LoadUsersList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка редактирования: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private void UpdateUserInDatabase(int userId, string password, string role)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection("Data Source=Messenger.db"))
                {
                    conn.Open();

                    SqliteCommand cmd = conn.CreateCommand();

                    cmd.CommandText = $"UPDATE user SET userPassword = '{password}', userRole = '{role}' " + $"WHERE userId = {userId}";

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        MessageBox.Show("Пользователь не найден или данные не изменились", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления в базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Метод получения пользователя по имени из БД
        /// </summary>
        private int GetUserIdByName(string userName)
        {
            try
            {
                SqliteConnection conn = new SqliteConnection("Data Source=Messenger.db");
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT userId FROM user WHERE userName = '{userName}'";

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        // Перемещение по настройкам
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
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            scroller.ScrollToBottom();
        }
        // Закрытие настроек
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class UserInfo
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public int Rating { get; set; }
    }
}
