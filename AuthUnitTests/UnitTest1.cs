using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TOP_Messanger;

namespace AuthUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        #region Проверка на пустоту логина и пароля
        // Проверка на пустоту логина
        [TestMethod]
        public void CheckLoginAndPassword_EmptyLogin_IsValidFalse_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckLoginAndPassword("", "123");

            Assert.IsFalse(regResult.IsValid);
        }
        // Проверка на пустоту пароля
        [TestMethod]
        public void CheckLoginAndPassword_EmptyPassword_IsValidFalse_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckLoginAndPassword("test", "");

            Assert.IsFalse(regResult.IsValid);
        }
        // Проверка на пустоту логина и пароля
        [TestMethod]
        public void CheckLoginAndPassword_EmptyLoginAndEmptyPassword_IsValidFalse_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckLoginAndPassword("", "");

            Assert.IsFalse(regResult.IsValid);
        }
        #endregion

        #region Проверка на подключение сервера
        // Проверка на неправильный логин сервера
        [TestMethod]
        public void CheckLoginAndPassword_IncorectServerLoginAndCorectServerPassword_IsServerFalse_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckLoginAndPassword("test", "pAv0Pav183");

            Assert.IsFalse(regResult.IsServer);
        }
        // Проверка на неправильный пароль сервера
        [TestMethod]
        public void CheckLoginAndPassword_CorectServerLoginAndIncorectServerPassword_IsServerFalse_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckLoginAndPassword("server", "test");

            Assert.IsFalse(regResult.IsServer);
        }
        // ПЕРЕПРОВЕРИТЬ
        // Проверка на правильный логин и пароль сервера
        [TestMethod]
        public void CheckLoginAndPassword_CorectServerLoginAndCorectServerPassword_IsServerTrue_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckLoginAndPassword("server", "pAv0Pav183");

            Assert.IsTrue(regResult.IsServer);
        }
        #endregion

        #region Проверка на гостя
        // Проверка на незаполненный логин
        [TestMethod]
        public void CheckGuestLogin_EmptyGuestLogin_IsValidFalse_IsGuestFalse_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckGuestLogin("");

            Assert.IsFalse(regResult.IsValid);
            Assert.IsFalse(regResult.IsGuest);
        }
        // Проверка на гостевой логин
        [TestMethod]
        public void CheckGuestLogin_GuestLogin_IsValidTrue_IsGuestTrue_Test()
        {
            RegistrationResult regResult;
            Registration registration = new Registration();

            regResult = registration.CheckGuestLogin("test");

            Assert.IsTrue(regResult.IsValid);
            Assert.IsTrue(regResult.IsGuest);
        }
        #endregion

        /*
        #region Проверка на сброс сессии
        // Проверка на сброс сессии
        [TestMethod]
        public void ResetSession_CurrentLoginIsGuestIsServer_IsFalseCurrent_IsFalseLogin_IsFalseIsGuestIsServer_Test()
        {
            Registration registration = new Registration();

            Registration.CurrentLogin = "Test";
            Registration.IsGuest = true;
            Registration.IsServer = true;

            registration.ResetSession();

            Assert.IsTrue(string.IsNullOrEmpty(Registration.CurrentLogin));
            Assert.IsFalse(Registration.IsGuest);
            Assert.IsFalse(Registration.IsServer);
        }
        #endregion
        */

        #region Остановка сервера
        // Проверка на работу остановки сервера
        [TestMethod]
        public void StopServer_IsServerRunning_IsServerRunningFalse_Test()
        {
            Registration registration = new Registration();
            Registration.IsServerRunning = true;

            Registration.StopServer();

            Assert.IsFalse(Registration.IsServerRunning);
        }

        // Проверка на остановленный сервер
        [TestMethod]
        public void StopServer_NotIsServerRunning_IsServerRunningFalse_Test()
        {
            Registration registration = new Registration();
            Registration.IsServerRunning = false;

            Registration.StopServer();

            Assert.IsFalse(Registration.IsServerRunning);
        }
        #endregion
    }
}
