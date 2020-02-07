using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientApp.addWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.models;

namespace ClientApp.addWindows.Tests
{
    [TestClass()]
    public class AddJokeWindowTests
    {
        private static AddJokeWindow addJokeWindow = new AddJokeWindow();
        private static string testingString = "testing string";
        private static string emptyString = "";
        private static Joke [] jokesArray = { 
            new Joke() { Content = testingString, Title = "Title", AuthorFK = 1, Id = 1, CreatedDate = DateTime.Now },
            new Joke() { Content = "Content", Title = "Title", AuthorFK = 1, Id = 1, CreatedDate = DateTime.Now }
        };
        private static List<Joke> jokes = new List<Joke>(jokesArray);

        [TestMethod()]
        public void CheckNewJokeTitleCorrectTest()
        {
            Assert.IsTrue(addJokeWindow.CheckNewJokeTitle(testingString));
        }

        [TestMethod()]
        public void CheckNewJokeTitleFailTest()
        {
            Assert.IsFalse(addJokeWindow.CheckNewJokeTitle(emptyString));
        }

        [TestMethod()]
        public void CheckNewJokeContentCorrectTest()
        {
            Assert.IsTrue(addJokeWindow.CheckNewJokeContent(testingString));
        }

        [TestMethod()]
        public void CheckNewJokeContentFailTest()
        {
            Assert.IsFalse(addJokeWindow.CheckNewJokeContent(emptyString));
        }

        [TestMethod()]
        public void CheckUniqueJokeContentFailTest()
        {
            Assert.IsFalse(addJokeWindow.CheckUniqueJokeContent(jokes, testingString));
        }

        [TestMethod()]
        public void CheckUniqueJokeContentCorrectTest()
        {
            Assert.IsTrue(addJokeWindow.CheckUniqueJokeContent(jokes, "Unique string"));
        }
    }
}