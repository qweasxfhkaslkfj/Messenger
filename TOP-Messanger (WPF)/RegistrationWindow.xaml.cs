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
        }
        // Кнопки входа
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(loginTextBox.Text))
                MessageBox.Show("Пожалуйста, введите логин", 
                                "Ошибка входа", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Information);
            else if (String.IsNullOrEmpty(passwordTextBox.Password))
                MessageBox.Show("Пожалуйста, введите пароль",
                                "Ошибка входа",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            else
                UserEnter();
        }
        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(loginTextBox.Text))
                GuestEnter();
            else
                MessageBox.Show("Пожалуйста, введите логин",
                                "Ошибка входа",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
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

        // Метод входа для гостя
        private void GuestEnter()
        {
            var result = registration.CheckGuestLogin(loginTextBox.Text);

            if (result.IsValid)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка входа! \nПожалуйста, повторите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Метод входа для пользователя
        private void UserEnter()
        {
            string password = passwordTextBox.Password.ToString();
            var result = registration.CheckLoginAndPassword(
                    loginTextBox.Text,
                    password);

            if (result.IsValid)
            {
                if (Registration.IsServer)
                {
                    MessageBox.Show("Выполнен вход как сервер");
                }
                else
                {
                    MessageBox.Show("Выполнен вход как пользователь");
                }

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
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
