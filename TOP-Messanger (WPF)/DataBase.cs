using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TOP_Messanger
{
    internal class DataBase
    {
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
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public static void StartUserTable()
        {
            try
            {
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
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
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
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
        public static void InsertMessageTable(DateTime messageTime, string messageText, bool isPrivate, string sender, string getter)
        {
            try
            {
                conn = new SqliteConnection();
                SqliteCommand cmd = new SqliteCommand();

                cmd.Connection = conn;
                cmd.CommandText = $@"INSERT INTO user (messageTime, messageText, isPrivate, sender, getter) VALUES 
                                                      ('{messageTime}', '{messageText}', {isPrivate}, '{sender}', '{getter}');";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public static void ConnectDB()
        {

        }
    }
}
