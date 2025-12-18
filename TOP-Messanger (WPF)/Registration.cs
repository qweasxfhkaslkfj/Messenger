using System;
using System.Collections.Generic;

namespace TOP_Messanger
{
    internal class Registration
    {
        public static string userLogin;
        public static string CurrentLogin { get; private set; } = "";
        public static bool IsGuest { get; private set; } = false;
        public static bool IsServer { get; private set; } = false;

        private Dictionary<string, string> usersLog;
        private const string ServerAdminLogin = "server";
        private const string AdminPassword = "pAv0Pav183";

        // Конструктор Registration
        public Registration()
        {
            usersLog = new Dictionary<string, string>();
            UsersLogins();
        }
        // Список логинов и паролей
        private void UsersLogins()
        {
            usersLog.Add("krs333", "krs123");
            usersLog.Add("Pagan821", "ars123");
            usersLog.Add("denden", "denzem123");
            usersLog.Add("cat_noir", "denzol123");
            usersLog.Add("lady_bug", "kerya123");
            usersLog.Add("tabeer", "alb123");
            usersLog.Add("lushPush", "ol123");
            usersLog.Add("Siles", "zah123");
            usersLog.Add("USF055", "usf123");
            usersLog.Add("vld666", "vld123");
            usersLog.Add("ananas", "nast123");
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

        // Проверка пользовательской регистрации
        public RegistrationResult CheckLoginAndPassword(string login, string password)
        {
            ResetSession();

            if (String.IsNullOrWhiteSpace(login) || 
                String.IsNullOrWhiteSpace(password))
            {
                return new RegistrationResult { IsValid = false };
            }    

            // Проверка подключения сервера
            if (login == ServerAdminLogin && password == AdminPassword)
            {
                CurrentLogin = login;
                userLogin = login;
                IsServer = true;
                IsGuest = false;

                return new RegistrationResult
                {
                    IsValid = true,
                    Login = login,
                    IsServer = true,
                    IsGuest = false
                };
            }

            // Проверка подключения обычных пользователей
            if (usersLog.ContainsKey(login) && 
                usersLog[login] == password)
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

            // Неверные данные
            return new RegistrationResult
            {
                IsValid = false
            };
        }
        // Проверка гостевой регистрации
        public RegistrationResult CheckGuestLogin(string login)
        {
            ResetSession();

            if (string.IsNullOrWhiteSpace(login))
                return new RegistrationResult { IsValid = false };

            CurrentLogin = login;
            userLogin= login;
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
    }

    public class RegistrationResult
    {
        public bool IsValid { get; set; }
        public bool IsGuest { get; set; }
        public bool IsServer { get; set; }
        public string Login { get; set; }
    }
}
