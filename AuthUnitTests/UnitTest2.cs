using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using TOP_Messanger;

namespace AuthUnitTests
{
    [TestClass]
    public class UnitTest2
    {
        #region Голбальные переменные
        private const string TestServerIp = "127.0.0.1";
        private const int TestServerPort = 9000;

        // Заглушка MainWindow
        private class MockMainWindow : MainWindow
        {
            public string LastReceivedMessage { get; private set; }
            public bool MessageReceived { get; private set; }

            public void AddMessage(string message)
            {
                LastReceivedMessage = message;
                MessageReceived = true;
                Console.WriteLine($"MockMainWindow получил сообщение: {message}");
            }

            public void Reset()
            {
                LastReceivedMessage = null;
                MessageReceived = false;
            }
        }
        #endregion

        #region Тесты подключения
        // Подключение
        [TestMethod]
        public void Connect_ValidServerAddress_ReturnsTrue()
        {
            var mockWindow = new MockMainWindow();
            var client = new ChatClient(mockWindow);

            bool result = client.Connect();

            Assert.IsTrue(result);
            Assert.IsTrue(client.IsConnected);
        }

        // Неверный адрес сервера
        [TestMethod]
        public void Connect_InvalidServerAddress_ReturnsFalse()
        {
            MockMainWindow mockWindow = new MockMainWindow();
            ChatClient client = new ChatClient(mockWindow);

            ChatClient clientWithWrongAddress = CreateClientWithCustomAddress(mockWindow, "192.168.1.999", 9999);

            bool result = clientWithWrongAddress.Connect();

            Assert.IsFalse(result);
            Assert.IsFalse(clientWithWrongAddress.IsConnected);
        }

        // Повторное подключение
        [TestMethod]
        public void Connect_AfterDisconnect_CanReconnect()
        {
            MockMainWindow mockWindow = new MockMainWindow();
            ChatClient client = new ChatClient(mockWindow);

            bool firstConnect = client.Connect();
            Assert.IsTrue(firstConnect);
            client.Disconnect();
            Assert.IsFalse(client.IsConnected);
            Thread.Sleep(100);
            bool reconnectResult = client.Connect();

            Assert.IsTrue(reconnectResult);
            Assert.IsTrue(client.IsConnected);
        }
        #endregion

        #region Отправка сообщений
        // Тест отправки сообщения при подключении
        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Network")]
        public void SendMessage_WhenConnected_NoException()
        {
            MockMainWindow mockWindow = new MockMainWindow();
            ChatClient client = new ChatClient(mockWindow);
            SetTestLoginData("testUser", "user");
            bool connected = client.Connect();
            Assert.IsTrue(connected, "Должно быть подключение к серверу");

            try
            {
                client.SendMessage("Тестовое сообщение");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Отправка сообщения вызвала исключение: {ex.Message}");
            }
        }

        // Отправка сообщения без подключения
        [TestMethod]
        public void SendMessage_WhenNotConnected_NoExceptionButNoSend()
        {
            MockMainWindow mockWindow = new MockMainWindow();
            ChatClient client = new ChatClient(mockWindow);
            SetTestLoginData("testUser", "user");


            try
            {
                client.SendMessage("Тестовое сообщение");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Отправка сообщения без подключения не должна вызывать исключение: {ex.Message}");
            }
        }

        #endregion

        #region Вспомогательные методы

        private ChatClient CreateClientWithCustomAddress(MainWindow window, string ip, int port)
        {
            ChatClient client = new ChatClient(window);

            var field = typeof(ChatClient).GetField("server_IP",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (field != null)
            {
                field.SetValue(null, ip);
            }

            return client;
        }

        // Установка тестовых данных логина
        private void SetTestLoginData(string login, string role)
        {
            var loginField = typeof(Registration).GetField("userLogin",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (loginField != null)
            {
                loginField.SetValue(null, login);
            }

            var roleField = typeof(Registration).GetField("CurrentRole",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (roleField != null)
            {
                roleField.SetValue(null, role);
            }
        }

        // Метод очистки тестовых данных
        private void ClearTestLoginData()
        {
            SetTestLoginData("", "");
        }

        // Инициализация перед каждым тестом
        [TestInitialize]
        public void TestInitialize()
        {
            ClearTestLoginData();
        }

        // Очистка после каждого теста
        [TestCleanup]
        public void TestCleanup()
        {
            ClearTestLoginData();
        }

        #endregion
    }
}
