using System;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace TOP_Messanger
{
    internal class Registration
    {
        public static string userLogin;
        public static string CurrentLogin { get; private set; } = "";
        public static bool IsGuest { get; private set; } = false;
        public static bool IsServer { get; private set; } = false;
        public static bool IsServerRunning { get; set; } = false;

        private const string ServerAdminLogin = "server";
        private const string AdminPassword = "pAv0Pav183";

        // Конструктор Registration
        public Registration()
        {
            if (!System.IO.File.Exists("Messenger.db"))
            {
                DataBase.CreateDB();
                DataBase.StartUserTable();
            }
        }

        // Роль участника
        public static string CurrentRole
        {
            get
            {
                if (IsServer)
                    return "Server";
                else if (IsGuest)
                    return "Guest";
                else
                    return "User";
            }
        }

        // Проверка пользователя
        public RegistrationResult CheckLoginAndPassword(string login, string password)
        {
            ResetSession();

            if (String.IsNullOrWhiteSpace(login) ||
                String.IsNullOrWhiteSpace(password))
            {
                return new RegistrationResult { IsValid = false };
            }

            if (login == ServerAdminLogin && password == AdminPassword)
            {
                CurrentLogin = login;
                userLogin = login;
                IsServer = true;
                IsGuest = false;

                if (!IsServerRunning)
                {
                    ChatServer.Instance.Start();
                    IsServerRunning = true;
                }

                return new RegistrationResult
                {
                    IsValid = true,
                    Login = login,
                    IsServer = true,
                    IsGuest = false
                };
            }

            try
            {
                if (ValidateUserInDatabase(login, password))
                {
                    CurrentLogin = login;
                    userLogin = login;
                    IsGuest = false;
                    IsServer = false;

                    return new RegistrationResult
                    {
                        IsValid = true,
                        Login = login,
                        IsServer = false,
                        IsGuest = false
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке пользователя в БД: {ex.Message}");
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new RegistrationResult
            {
                IsValid = false
            };
        }

        // Проверка через базу данных
        private bool ValidateUserInDatabase(string login, string password)
        {
            try
            {
                using (var conn = new SqliteConnection($"Data Source={DataBase.connStr}"))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"SELECT userId FROM user WHERE userName = '{login}' AND userPassword = '{password}'";

                    var result = cmd.ExecuteScalar();
                    return result != null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Проверка гостя
        public RegistrationResult CheckGuestLogin(string login)
        {
            ResetSession();

            if (string.IsNullOrWhiteSpace(login))
                return new RegistrationResult { IsValid = false };

            CurrentLogin = login;
            userLogin = login;
            IsGuest = true;
            IsServer = false;

            return new RegistrationResult
            {
                IsValid = true,
                Login = login,
                IsGuest = true
            };
        }

        // Сброс прошлой сессии
        private void ResetSession()
        {
            CurrentLogin = "";
            IsGuest = false;
            IsServer = false;
        }

        // Остановка сервера
        public static void StopServer()
        {
            if (IsServerRunning)
            {
                ChatServer.Instance.Stop();
                IsServerRunning = false;
            }
        }
    }

    public class RegistrationResult
    {
        public bool IsValid { get; set; }
        public bool IsGuest { get; set; }
        public bool IsServer { get; set; }
        public string Login { get; set; }
    }
}