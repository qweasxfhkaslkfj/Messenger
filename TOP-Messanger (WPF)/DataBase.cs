using Microsoft.Data.Sqlite;
using System;
<<<<<<< HEAD
using System.Windows;
=======
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241

namespace TOP_Messanger
{
    internal class DataBase
    {
<<<<<<< HEAD
        public static string connStr = "Messenger.db";
        static SqliteConnection conn;

        // Первое создание БД
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
=======
        static SqliteConnection conn;
        static string connStr = "Messenger.db";


        public static void CreateDB()
        {
            conn = new SqliteConnection();
            try
            {
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE user (userId INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                       userName STRING NOT NULL UNIQUE
                                                       userPassword STRING NOT NULL
                                                       userRole STRING NOT NULL
                                                       userRating INTEGER NOT NULL
                                                       userColor STRING NOT NULL);";

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE message (messageId INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                          messageTime DATETIME NOT NULL
                                                          messageText TEXT NOT NULL
                                                          isPrivate BOOL NOT NULL
                                                          sender STRING NOT NULL
                                                          getter STRING NULL);";
>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
<<<<<<< HEAD
                if (conn != null)
                    conn.Close();
            }
        }
=======
                conn.Close();
            }
        }

>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241
        public static void StartUserTable()
        {
            try
            {
<<<<<<< HEAD
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
=======
                conn = new SqliteConnection();
                SqliteCommand cmd = new SqliteCommand();

                cmd.Connection = conn;
                cmd.CommandText = @"INSERT INTO user (userName, userPassword, userRole, userRating, userColor) VALUES 
                                                     ('server', 'pAv0Pav183', 'server', 0, '#00a550'),
                                                     ('krs333', 'krs123', 'user', 0, '#00a550'),
                                                     ('Pagan821', 'ars123', 'user', 0, '#00a550'),
                                                     ('denden', 'denzem123', 'user', 0, '#00a550'),
                                                     ('cat_noir', 'denzol123', 'user', 0, '#00a550'),
                                                     ('lady_bug', 'kerya123', 'user', 0, '#00a550'),
                                                     ('tabeer', 'alb123', 'user', 0, '#00a550'),
                                                     ('lushPush', 'ol123', 'user', 0, '#00a550'),
                                                     ('Siles', 'zah123', 'user', 0, '#00a550'),
                                                     ('USF055', 'usf123', 'user', 0, '#00a550'),
                                                     ('vld666', 'vld123', 'user', 0, '#00a550'),
                                                     ('ananas', 'nast123', 'user', 0, '#00a550');";
>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
<<<<<<< HEAD
                if (conn != null)
                    conn.Close();
            }
        }

        // Добавление в таблицу
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
=======
                conn.Close();
            }
        }

        public static void InsertUserTable(string userName, string password, string role, string color)
        {
            if (role != "user" || role != "server")
                return;

            try
            {
                conn = new SqliteConnection();
                SqliteCommand cmd = new SqliteCommand();

                cmd.Connection = conn;
                cmd.CommandText = $@"INSERT INTO user (userName, userPassword, userRole, userRating, userColor) VALUES 
                                                     ('{userName}', '{password}', '{role}', 0, '{color}');";
>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
<<<<<<< HEAD
                if (conn != null)
                    conn.Close();
=======
                conn.Close();
>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241
            }
        }
        public static void InsertMessageTable(DateTime messageTime, string messageText, bool isPrivate, string sender, string getter)
        {
            try
            {
<<<<<<< HEAD
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
=======
                conn = new SqliteConnection();
                SqliteCommand cmd = new SqliteCommand();

                cmd.Connection = conn;
                cmd.CommandText = $@"INSERT INTO user (messageTime, messageText, isPrivate, sender, getter) VALUES 
                                                      ('{messageTime}', '{messageText}', {isPrivate}, '{sender}', '{getter}');";
>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
<<<<<<< HEAD
                if (conn != null)
                    conn.Close();
            }
        }

        // Удаление из таблицы
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

        // Проверка пользователя
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
=======
                conn.Close();
            }
        }

        public static void ConnectDB()
        {

        }
    }
}
>>>>>>> 95a36f7a3cf780ea6d4903c0060b2b1054a2a241
