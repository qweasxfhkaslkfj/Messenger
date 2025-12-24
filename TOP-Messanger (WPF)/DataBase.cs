using Microsoft.Data.Sqlite;
using System;
using System.Windows;

namespace TOP_Messanger
{
    /// <summary>
    /// Класс, для создания базы данных Messenger.db
    /// </summary>

    internal class DataBase
    {
        public static string connStr = "Messenger.db";
        static SqliteConnection conn;

        /// <summary>
        /// Процедура инициализирующая создание БД и нужных таблиц
        /// </summary>
        public static void CreateDB()
        {
            try
            {
                conn = new SqliteConnection($"Data Source={connStr}");
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS user (userId INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                                     userName TEXT NOT NULL UNIQUE,
                                                                     userPassword TEXT NOT NULL,
                                                                     userRole TEXT NOT NULL,
                                                                     userRating INTEGER NOT NULL,
                                                                     userColor TEXT NOT NULL,
                                                                     userIP TEXT NOT NULL);";

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS message (messageId INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                                        messageTime DATETIME NOT NULL,
                                                                        messageText TEXT NOT NULL,
                                                                        isPrivate BOOL NOT NULL,
                                                                        sender TEXT NOT NULL,
                                                                        getter TEXT NULL);";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        /// <summary>
        /// Процедура устанавливающая начальные данные
        /// </summary>
        public static void StartUserTable()
        {
            try
            {
                conn = new SqliteConnection($"Data Source={connStr}");
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"INSERT OR IGNORE INTO user (userName, userPassword, userRole, userRating, userColor, userIP) VALUES 
                                                               ('server', 'pAv0Pav183', 'server', 0, '#808080', '192.168.88.145'),
                                                               ('krs333', 'krs123', 'user', 0, '#FF0000', '192.168.88.0'),
                                                               ('Pagan821', 'ars123', 'user', 0, '#00FF00', '192.168.88.0'),
                                                               ('denden', 'denzem123', 'user', 0, '#0000FF', '192.168.88.0'),
                                                               ('cat_noir', 'denzol123', 'user', 0, '#00CED1', '192.168.88.0'),
                                                               ('lady_bug', 'kerya123', 'user', 0, '#000080', '192.168.88.0'),
                                                               ('tabeer', 'alb123', 'user', 0, '#FF00FF', '192.168.88.0'),
                                                               ('lushPush', 'ol123', 'user', 0, '#228B22', '192.168.88.0'),
                                                               ('Siles', 'zah123', 'user', 0, '#FFA500', '192.168.88.0'),
                                                               ('USF055', 'usf123', 'user', 0, '#8B4513', '192.168.88.0'),
                                                               ('vld666', 'vld123', 'user', 0, '#FF1493', '192.168.88.0'),
                                                               ('ananas', 'nast123', 'user', 0, '#800080', '192.168.88.0');";
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// Процедуры добавления данных в БД
        /// </summary>
        public static void InsertUserTable(string userName, string password, string role, string color, string ip)
        {
            if (role != "user" && role != "server")
            {
                MessageBox.Show("Неверно указанная роль");
                return;
            }

            try
            {
                conn = new SqliteConnection($"Data Source={connStr}");
                SqliteCommand cmd = new SqliteCommand();

                cmd.Connection = conn;
                cmd.CommandText = $@"INSERT INTO user (userName, userPassword, userRole, userRating, userColor, userIP) VALUES 
                                                      ('{userName}', '{password}', '{role}', 0, '{color}', '{ip}');";
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        public static void InsertMessageTable(DateTime messageTime, string messageText, bool isPrivate, string sender, string getter)
        {
            try
            {
                conn = new SqliteConnection($"Data Source={connStr}");
                SqliteCommand cmd = new SqliteCommand();

                cmd.Connection = conn;
                string formattedTime = messageTime.ToString("yyyy-MM-dd HH:mm:ss");
                string getterValue;

                if (string.IsNullOrEmpty(getter))
                    getterValue = "NULL";
                else
                    getterValue = $"'{getter}'";

                cmd.CommandText = $@"INSERT INTO message (messageTime, messageText, isPrivate, sender, getter) VALUES 
                                                         ('{formattedTime}', '{messageText}', {isPrivate}, '{sender}', {getterValue});";
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// Процедуры удаления данных в БД
        /// </summary>
        public static void DeleteFromUserTable(int userId)
        {
            try
            {
                conn = new SqliteConnection($"Data Source={connStr}");
                conn.Open();

                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"DELETE FROM user WHERE userId = {userId}";

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected <= 0)
                    MessageBox.Show($"Пользователь с ID {userId} не найден");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        public static void DeleteFromMessageTable(int messageId)
        {
            try
            {
                conn = new SqliteConnection($"Data Source={connStr}");
                conn.Open();

                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"DELETE FROM message WHERE messageId = {messageId}";

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    MessageBox.Show($"Сообщение с ID {messageId} успешно удалено");
                else
                    MessageBox.Show($"Сообщение с ID {messageId} не найдено");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении сообщения: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// Метод проверки пользователей
        /// </summary>
        public static bool ValidateUser(string userName, string password)
        {
            SqliteConnection conn = null;
            try
            {
                conn = new SqliteConnection($"Data Source={connStr}");
                conn.Open();

                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT userId FROM user WHERE userName = '{userName}' AND userPassword = '{password}'";

                var result = cmd.ExecuteScalar();
                return result != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки пользователя: {ex.Message}");
                return false;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}