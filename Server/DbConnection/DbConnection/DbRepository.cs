using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbConnection
{
    public static class DbRepository
    {
        private static readonly MyDbContext context = new MyDbContext();

        public static User GetUserByNicknameAndPassword(string nickname, string password)
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

        public static User GetUserByNickname(string nickname)
        {
            try
            {
                User user = context.Users.Where(u => u.Nickname == nickname).Single();
                return user;
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }

        public static bool AddUser(User user)
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

        public static bool AddJoke(Joke joke)
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

        public static bool AddComment(Comment comment)
        {
            context.Comments.Add(comment);
            context.SaveChanges();
            return true;
        }

        public static bool UpdateUser(User newUser)
        {
            try
            {
                User oldUser = context.Users.Find(newUser.Id);
                oldUser.Name = newUser.Name;
                oldUser.Surname = newUser.Surname;
                oldUser.Nickname = newUser.Nickname;
                oldUser.Email = newUser.Email;
                oldUser.Password = newUser.Password;
                context.SaveChanges();
                return true;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return false;
            }
        }

        public static bool UpdateJoke(Joke newJoke)
        {
            try
            {
                Joke oldJoke = context.Jokes.Find(newJoke.Id);
                oldJoke.Title = newJoke.Title;
                oldJoke.Content = newJoke.Content;
                oldJoke.CreatedDate = newJoke.CreatedDate;
                oldJoke.Author = newJoke.Author;
                oldJoke.AuthorFK = newJoke.AuthorFK;
                context.SaveChanges();
                return true;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return false;
            }
        }

        public static bool UpdateComment(Comment newComment)
        {
            try
            {
                Comment oldComment = context.Comments.Find(newComment.Id);
                oldComment.Content = newComment.Content;
                oldComment.CreatedDate = newComment.CreatedDate;
                oldComment.Author = newComment.Author;
                oldComment.AuthorFK = newComment.AuthorFK;
                context.SaveChanges();
                return true;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return false;
            }
        }

        public static bool DeleteUser(int userId)
        {
            try
            {
                context.Remove(context.Users.Find(userId));
                context.SaveChanges();
                return true;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return false;
            }
        }

        public static bool DeleteJoke(int jokeId)
        {
            try
            {
                context.Remove(context.Jokes.Find(jokeId));
                context.SaveChanges();
                return true;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return false;
            }
        }

        public static bool DeleteComment(int commentId)
        {
            try
            {
                context.Remove(context.Comments.Find(commentId));
                context.SaveChanges();
                return true;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return false;
            }
        }

        public static List<Joke> GetAllJokes()
        {
            return context.Jokes.ToList();
        }

        public static List<Joke> GetUsersJokes(int userId)
        {
            return context.Jokes.Where(j => j.AuthorFK == userId).ToList();
        }

        public static List<Comment> GetUsersComments(int userId)
        {
            return context.Comments.Where(c => c.AuthorFK == userId).ToList();
        }

        public static List<Comment> GetJokesComments(int jokeId)
        {
            return context.Comments.Where(c => c.JokeFK == jokeId).ToList();
        }

        public static List<Joke> GetJokesAbout(string searchText)
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

        public static Joke GetRandomJoke()
        {
            var jokes = context.Jokes.ToArray();
            return jokes[new Random().Next(jokes.Length)];
        }

        public static List<User> GetAllUsers()
        {
            return context.Users.ToList();
        }

        public static List<Comment> GetAllComments()
        {
            return context.Comments.ToList();
        }

        public static User GetUserById(int userId)
        {
            try
            {
                User user = context.Users.Find(userId);
                return user;
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }

        public static Joke GetJokeById(int jokeId)
        {
            try
            {
                Joke joke = context.Jokes.Find(jokeId);
                return joke;
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }

        public static Comment GetCommentById(int commentId)
        {
            try
            {
                Comment comment = context.Comments.Find(commentId);
                return comment;
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }
    }
}
