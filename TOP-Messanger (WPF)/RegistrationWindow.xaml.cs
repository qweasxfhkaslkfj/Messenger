using System;
using System.Windows;
using System.Windows.Input;

namespace TOP_Messanger
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private Registration registration;

        public RegistrationWindow()
        {
            InitializeComponent();
            registration = new Registration();

            DataBase.CreateDB();
            DataBase.StartUserTable();
        }
        // Кнопки входа
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text.Trim();
            string password = passwordTextBox.Password;

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Пожалуйста, введите логин!", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите пароль!", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            RegistrationResult result = registration.CheckLoginAndPassword(login, password);

            if (result.IsValid)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text.Trim();

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Пожалуйста, введите логин", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = registration.CheckGuestLogin(login);

            if (result.IsValid)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка входа");
            }

        }
        private void showPasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (passwordTextBox.Visibility == Visibility.Visible)
            {
                passwordVisibleTextBox.Text = passwordTextBox.Password;
                passwordTextBox.Visibility = Visibility.Collapsed;
                passwordVisibleTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                passwordTextBox.Password = passwordVisibleTextBox.Text;
                passwordVisibleTextBox.Visibility = Visibility.Collapsed;
                passwordTextBox.Visibility = Visibility.Visible;
            }
        }

        // Обработчик нажатия на Enter
        private void LoginTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                passwordTextBox.Focus();
                e.Handled = true;
            }
        }
        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginBtn_Click(sender, e);
                e.Handled = true;
            }
        }
        private void PasswordVisibleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginBtn_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
