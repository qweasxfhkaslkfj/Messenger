using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Windows;

namespace TOP_Messanger
{
    internal class ChatServer
    {
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        private bool isRunning = false;
        private Thread serverThread;
        private Thread broadcastThread;

        private const int port = 9000;
        private Queue<string> messageQueue = new Queue<string>();
        private object lockObject = new object();

        private static ChatServer instance;
        public static ChatServer Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChatServer();
                return instance;
            }
        }

        private ChatServer() { }

        /// <summary>
        /// Процедура запуска сервера
        /// </summary>
        public void Start()
        {
            try
            {
                if (isRunning) return;

                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                isRunning = true;

                serverThread = new Thread(new ThreadStart(ListenForClients));
                serverThread.IsBackground = true;
                serverThread.Start();

                broadcastThread = new Thread(new ThreadStart(BroadcastMessages));
                broadcastThread.IsBackground = true;
                broadcastThread.Start();

                Console.WriteLine($"Сервер запущен на порту {port}");
                EnqueueMessage("[Сервер] Сервер запущен");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска сервера: {ex.Message}", "Ошибка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Процедура запуска слушателя клиентов
        /// </summary>
        private void ListenForClients()
        {
            try
            {
                while (isRunning)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    lock (clients)
                    {
                        clients.Add(client);
                    }

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.IsBackground = true;
                    clientThread.Start(client);

                    string clientInfo = $"Клиент подключен: {client.Client.RemoteEndPoint}";
                    Console.WriteLine(clientInfo);
                    EnqueueMessage($"[Сервер] Новый пользователь подключился");
                }
            }
            catch (Exception) { }
        }
        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (client.Connected && isRunning)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message.StartsWith("LOGIN:"))
                    {
                        string[] parts = message.Split(':');
                        if (parts.Length >= 2)
                        {
                            string login = parts[1];
                            string role = parts.Length >= 3 ? parts[2] : "User";
                            EnqueueMessage($"[Сервер] {login} ({role}) вошел в чат");
                        }
                    }
                    else if (message.StartsWith("MESSAGE:"))
                    {
                        string[] parts = message.Split(':');
                        if (parts.Length >= 3)
                        {
                            string login = parts[1];
                            string text = parts[2];
                            string censoredText = Censor.Censoring(text);
                            string fullMessage = $"[{DateTime.Now:HH:mm}] {login}: {censoredText}";
                            EnqueueMessage(fullMessage);
                        }
                    }
                    else
                    {
                        EnqueueMessage(message);
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                lock (clients)
                {
                    clients.Remove(client);
                }
                client.Close();
                EnqueueMessage($"[Сервер] Пользователь отключился");
            }
        }
        /// <summary>
        /// Процедура добавления сообщений
        /// </summary>
        public void EnqueueMessage(string message)
        {
            lock (lockObject)
            {
                messageQueue.Enqueue(message);
            }
        }
        /// <summary>
        /// Процедура рассылки сообщений
        /// </summary>
        private void BroadcastMessages()
        {
            while (isRunning)
            {
                try
                {
                    string message = null;

                    lock (lockObject)
                    {
                        if (messageQueue.Count > 0)
                            message = messageQueue.Dequeue();
                    }

                    if (message != null)
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);

                        List<TcpClient> clientsToRemove = new List<TcpClient>();

                        lock (clients)
                        {
                            foreach (TcpClient client in clients)
                            {
                                if (client.Connected)
                                {
                                    try
                                    {
                                        NetworkStream stream = client.GetStream();
                                        stream.Write(data, 0, data.Length);
                                    }
                                    catch (Exception)
                                    {
                                        clientsToRemove.Add(client);
                                    }
                                }
                                else
                                {
                                    clientsToRemove.Add(client);
                                }
                            }

                            foreach (TcpClient client in clientsToRemove)
                            {
                                clients.Remove(client);
                            }
                        }
                    }

                    Thread.Sleep(10);
                }
                catch (Exception) { }
            }
        }
        /// <summary>
        /// Процедура остановки сервера
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            EnqueueMessage("[Сервер] Сервер завершает работу");

            Thread.Sleep(100);

            if (listener != null)
            {
                listener.Stop();
            }

            lock (clients)
            {
                foreach (TcpClient client in clients)
                {
                    try
                    {
                        client.Close();
                    }
                    catch (Exception) { }
                }
                clients.Clear();
            }
        }

        public bool IsRunning
        {
            get 
            { 
                return isRunning; 
            }
        }
    }
}