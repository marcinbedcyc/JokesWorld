using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientApp.editWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.editWindows.Tests
{
    [TestClass()]
    public class CommentEditWindowTests
    {
        private static CommentEditWindow commentEditWindow = new CommentEditWindow();
        private static string testingString = "testing string";
        private static string emptyString = "";

        [TestMethod()]
        public void IsPreviousDateCorrectTest()
        {
            Assert.IsTrue(commentEditWindow.IsPreviousDate(DateTime.Now.AddDays(-3)));
        }

        [TestMethod()]
        public void IsPreviousDateFailTest()
        {
            Assert.IsFalse(commentEditWindow.IsPreviousDate(DateTime.Now.AddDays(3)));
        }

        [TestMethod()]
        public void IsContentEmptyCorrectTest()
        {
            Assert.IsTrue(commentEditWindow.IsContentEmpty(emptyString));
        }

        [TestMethod()]
        public void IsContentEmptyFailTest()
        {
            Assert.IsFalse(commentEditWindow.IsContentEmpty(testingString));
        }
    }
}