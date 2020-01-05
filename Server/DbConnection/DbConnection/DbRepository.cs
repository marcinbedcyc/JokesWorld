using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbConnection
{
    public class DbRepository
    {
        private MyDbContext context;

        public DbRepository() { context = new MyDbContext(); }

        public User GetUserWithNicknameAndPassword(string nickname, string password)
        {
            try
            {
                User user = context.Users.Where(u => u.Nickname == nickname).Single();
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    return user;
                else
                    return null;
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }

        public bool AddUser(User user)
        {
            try
            { 
                context.Users.Where(u => u.Nickname == user.Nickname).Single();
                context.Users.Where(u => u.Email == user.Email).Single();
                return false;
            }
            catch (System.InvalidOperationException)
            { 
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
        }

        public bool AddJoke(Joke joke)
        {
            try
            {
                context.Jokes.Where(j => j.Content == joke.Content).Single();
                return false;
            }
            catch (System.InvalidOperationException)
            {
                context.Jokes.Add(joke);
                context.SaveChanges();
                return true;
            }
        }

        public bool AddComment(Comment comment)
        {
            context.Comments.Add(comment);
            context.SaveChanges();
            return true;
        }

        public List<Joke> GetAllJokes()
        {
            return context.Jokes.ToList();
        }

        public List<Joke> GetUsersJokes(int userId)
        {
            return context.Users.Find(userId).Jokes.ToList();
        }

        public List<Comment> GetUsersComments(int userId)
        {
            return context.Users.Find(userId).Comments.ToList();
        }

        public List<Comment> GetJokesComments(int jokeId)
        {
            return context.Jokes.Find(jokeId).Comments.ToList();
        }

        public List<Joke> GetJokesAbout(string searchText)
        {
            try
            {
                return context.Jokes.Where(j => j.Title.ToLower().Contains(searchText.ToLower())).ToList();
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }

        public Joke GetRandomJoke()
        {
            return context.Jokes.Single(j => j.Title.Contains("jeżyku"));
        }

    }
}
