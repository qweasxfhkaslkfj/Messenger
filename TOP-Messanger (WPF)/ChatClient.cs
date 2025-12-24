using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace TOP_Messanger
{
    public class ChatClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private MainWindow mainWindow;
        private bool isConnected = false;

        private const string server_IP = "192.168.88.145"; // ПОМЕНЯТЬ НА "127.0.0.1", А ПОТОМ ВООБЩЕ НА 128 
        private const int port = 9000;

        public ChatClient(MainWindow window)
        {
            mainWindow = window;
        }

        /// <summary>
        /// Метод подключения к серверу
        /// </summary>
        public bool Connect()
        {
            try
            {
                client = new TcpClient();

                Thread.Sleep(100);

                client.Connect(server_IP, port);
                stream = client.GetStream();
                isConnected = true;

                receiveThread = new Thread(new ThreadStart(ReceiveMessages));
                receiveThread.IsBackground = true;
                receiveThread.Start();

                SendLoginInfo();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к серверу {server_IP}:{port}\nПричина: {ex.Message}", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Процедура получения логина
        /// </summary>
        private void SendLoginInfo()
        {
            string login = Registration.userLogin;
            string role = Registration.CurrentRole;
            string message = $"LOGIN:{login}:{role}";
            SendMessage(message);
        }

        /// <summary>
        /// Процедура отправки сообщения на сервер
        /// </summary>
        public void SendMessage(string message)
        {
            try
            {
                if (isConnected && stream != null)
                {
                    string formattedMessage = $"MESSAGE:{Registration.userLogin}:{message}";
                    byte[] data = Encoding.UTF8.GetBytes(formattedMessage);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
        }
        /// <summary>
        /// Процедура получения сообщений от сервера
        /// </summary>
        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (isConnected && stream != null)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (!receivedMessage.StartsWith("MESSAGE:") && !receivedMessage.StartsWith("LOGIN:"))
                    {
                        Action action = delegate
                        {
                            mainWindow.AddMessage(receivedMessage);
                        };

                        Application.Current.Dispatcher.Invoke(action);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка приема: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Процедура отключение от сервера
        /// </summary>
        public void Disconnect()
        {
            try
            {
                isConnected = false;

                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }

                if (client != null)
                {
                    client.Close();
                    client = null;
                }
            }
            catch (Exception) { }
        }

        public bool IsConnected
        {
            get { return isConnected; }
        }
    }
}