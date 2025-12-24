using System.Windows;
using System.Windows.Controls;

namespace TOP_Messanger
{
    public partial class AdminUserDialog : Window
    {
        private string mode;

        public string UserName
        {
            get { return LoginTextBox.Text.Trim(); }
        }
        public string Password
        {
            get { return PasswordBox.Password; }
        }
        public string Role
        {
            get
            {
                ComboBoxItem selectedItem = RoleComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                    return selectedItem.Content.ToString();

                return null;
            }
        }

        public AdminUserDialog(string mode)
        {
            InitializeComponent();
            this.mode = mode;
            InitializeDialog();
        }
        /// <summary>
        /// Процедура создания интерфейса
        /// </summary>
        private void InitializeDialog()
        {
            switch (mode)
            {
                case "add":
                    TitleText.Text = "Добавление пользователя";
                    ActionButton.Content = "Добавить";
                    PasswordGrid.Visibility = Visibility.Visible;
                    RoleGrid.Visibility = Visibility.Visible;
                    break;

                case "edit":
                    TitleText.Text = "Редактирование пользователя";
                    ActionButton.Content = "Сохранить";
                    PasswordGrid.Visibility = Visibility.Visible;
                    RoleGrid.Visibility = Visibility.Visible;
                    break;

                case "remove":
                    TitleText.Text = "Удаление пользователя";
                    ActionButton.Content = "Удалить";
                    WarningText.Text = "Внимание! Это действие необратимо. Все данные пользователя будут удалены.";
                    WarningText.Visibility = Visibility.Visible;
                    break;
            }
        }
        /// <summary>
        /// Обработчики событий нажатия на кнопки
        /// </summary>
        // Кнопка действия
        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                MessageBox.Show("Введите логин пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (mode != "remove" && string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }
        // Кнопка назад
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}