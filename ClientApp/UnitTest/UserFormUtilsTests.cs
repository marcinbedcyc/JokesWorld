using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Tests
{
    [TestClass()]
    public class UserFormUtilsTests
    {
        private string emptyString = "";
        private string name = "name";
        private string surname = "surname";
        private string nickname = "nickname";
        private string email = "nickname@email.com";
        private string badEmail = "nickname213email.com";
        private string password = "password";
        private string password2 = "password";
        private string wrongPassword = "wrongPassword";
        private List<models.User> users = new List<models.User>();

        [TestMethod()]
        public void CheckFormEmptyFormExceptionNameEmptyTest()
        {
            Assert.ThrowsException<EmptyFormException>(() => UserFormUtils.CheckForm(emptyString, surname, email, nickname, password, password2, users));
        }

        [TestMethod()]
        public void CheckFormEmptyFormExceptionSurnameEmptyTest()
        {
            Assert.ThrowsException<EmptyFormException>(() => UserFormUtils.CheckForm(name, emptyString, email, nickname, password, password2, users));
        }

        [TestMethod()]
        public void CheckFormEmptyFormExceptionEmailEmptyTest()
        {
            Assert.ThrowsException<EmptyFormException>(() => UserFormUtils.CheckForm(name, surname, emptyString, nickname, password, password2, users));
        }

        [TestMethod()]
        public void CheckFormEmptyFormExceptionNicknameEmptyTest()
        {
            Assert.ThrowsException<EmptyFormException>(() => UserFormUtils.CheckForm(name, surname, email, emptyString, password, password2, users));
        }

        [TestMethod()]
        public void CheckFormEmptyFormExceptionPasswordEmptyTest()
        {
            Assert.ThrowsException<EmptyFormException>(() => UserFormUtils.CheckForm(name, surname, email, nickname, emptyString, password2, users));
        }

        [TestMethod()]
        public void CheckFormEmptyFormExceptionRepeatedPasswordEmptyTest()
        {
            Assert.ThrowsException<EmptyFormException>(() => UserFormUtils.CheckForm(name, surname, email, nickname, password, emptyString, users));
        }

        [TestMethod()]
        public void CheckFormNotCorrectEmailExceptionTest()
        {
            Assert.ThrowsException<NotCorrectEmailException>(() => UserFormUtils.CheckForm(name, surname, badEmail, nickname, password, password2, users));
        }

        [TestMethod()]
        public void CheckFormDiffrentPasswordsExceptionTest1()
        {
            Assert.ThrowsException<DiffrentPasswordsException>(() => UserFormUtils.CheckForm(name, surname, email, nickname, password, wrongPassword, users));
        }

        [TestMethod()]
        public void CheckFormDiffrentPasswordsExceptionTest2()
        {
            Assert.ThrowsException<DiffrentPasswordsException>(() => UserFormUtils.CheckForm(name, surname, email, nickname, wrongPassword, password2, users));
        }

        [TestMethod()]
        public void CheckFormNicknameAlredyUsedExceptionTest()
        {
            models.User user = new models.User()
            {
                Nickname = nickname,
                Email = email
            };
            users.Add(user);
            Assert.ThrowsException<NicknameAlredyUsedException>(() => UserFormUtils.CheckForm(name, surname, email, nickname, password, password2, users));
        }

        [TestMethod()]
        public void CheckFormEmailAlredyUsedExceptionTest()
        {
            models.User user = new models.User()
            {
                Nickname = "uniqueNicknames",
                Email = email
            };
            users.Add(user);
            Assert.ThrowsException<EmailAlredyUsedException>(() => UserFormUtils.CheckForm(name, surname, email, nickname, password, password2, users));
        }

        [TestMethod()]
        public void CheckFormTest()
        {
            Assert.IsTrue(UserFormUtils.CheckForm(name, surname, email, nickname, password, password2, users));
        }

        [TestMethod()]
        public void IsValidEMailCorectTest()
        {
            Assert.IsTrue(UserFormUtils.IsValidEMail(email));
        }

        [TestMethod()]
        public void IsValidEMailFalseTest()
        {
            Assert.IsFalse(UserFormUtils.IsValidEMail(badEmail));
        }
    }
}