using System;

namespace DbConnection
{
    class Program
    {
        static void Main()
        {
            DbRepository repo = new DbRepository();
            AddSomeUsers(repo);
            User maricnbedcyc = repo.GetUserByNicknameAndPassword("marcinbedcyc", "password");
            User wlepciax = repo.GetUserByNicknameAndPassword("wlepciax", "password");
            //Console.WriteLine(maricnbedcyc.Id);
            //Console.WriteLine(wlepciax.Id);
            //AddSomeJokes(repo, ref maricnbedcyc);
            //AddSomeJokes2(repo, ref wlepciax);
            Joke joke = repo.GetRandomJoke();
            AddSomeComment(repo, ref joke, ref maricnbedcyc);

            //Comment comment = new Comment();
            //comment.Content = "Dobre! :)";
            //comment.Author = maricnbedcyc;
            //comment.Joke = joke;
            //comment.CreatedDate = DateTime.Now;
            //repo.AddComment(comment);

            //foreach (Joke j in repo.GetJokesAbout("Blondynk"))
            //foreach (Joke j in repo.GetJokesAbout("b"))
            //Console.WriteLine(maricnbedcyc.Jokes);
            //maricnbedcyc.Email = "cmartindev@gmail.com";
            if(repo.DeleteUser(wlepciax.Id))
                Console.WriteLine("Usunięto!\n\n");
            foreach (Joke j in repo.GetAllJokes())
            {
                Console.WriteLine($"Tytuł: {j.Title}");
                Console.WriteLine($"Autor: {j.Author.Nickname}");
                Console.WriteLine(j.Content);
                Console.WriteLine("\n\n");
            }

            foreach (Comment c in repo.GetUsersComments(maricnbedcyc.Id))
            {
                Console.WriteLine($"Autor: {c.Author.Nickname}");
                Console.WriteLine($"Dowcip: {c.Joke.Title}");
                Console.WriteLine(c.Content);
                Console.WriteLine("\n\n");
            }

            foreach (Comment c in repo.GetJokesComments(1))
            {
                Console.WriteLine($"Autor: {c.Author.Nickname}");
                Console.WriteLine($"Dowcip: {c.Joke.Title}");
                Console.WriteLine(c.Content);
                Console.WriteLine("\n\n");
            }

        }

        static private void AddSomeUsers(DbRepository repo)
        {
            User user = new User
            {
                Name = "Marcin",
                Surname = "Cyc",
                Email = "marcinbedcyc@gmail.com",
                Nickname = "marcinbedcyc",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
            repo.AddUser(user);

            User user1 = new User
            {
                Name = "Kornelia",
                Surname = "Nalepa",
                Email = "wlpeciax@gmail.com",
                Nickname = "wlepciax",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
            repo.AddUser(user1);
        }

        static private void AddSomeJokes(DbRepository repo, ref User author)
        {
            Joke joke = new Joke
            {
                Title = "2Blondynki i nagroda",
                Content = "Dwie blondynki wygrały nagrodę. Nie wiedzą co z nią zrobić, aż w końcu Zuza mówi:" +
            "-wiem, polecimy na słońce" +
            "- co Ty spalimy się" +
            "- przecierz polecimy w nocy.",
                Author = author,
                CreatedDate = DateTime.Now
            };
            repo.AddJoke(joke);

            Joke joke2 = new Joke
            {
                Title = "2Blondynki i mecz",
                Content = "Dwie blondynki oglądają mecz Brazylia vs Czechy,pierwsza poszla zrobic popcorn...druga krzyczy" +
            "- GOOOL" +
            "- kto strzelił ? " +
            "- Ronaldinho" +
            "za chwile" +
            "- jest drugi gol" +
            "- kto szczelił" +
            "- jakis Replay.",
                Author = author,
                CreatedDate = DateTime.Now
            };
            repo.AddJoke(joke2);

            Joke joke3 = new Joke
            {
                Title = "2Żydów i Watykan",
                Content = "Dwóch Żydów zwiedza Watykan, podziwiają przepych i bogactwo. Jeden mówi: " +
            "-Popatrz, a zaczynali od stajenki..",
                Author = author,
                CreatedDate = DateTime.Now
            };
            repo.AddJoke(joke3);
        }

        static private void AddSomeJokes2(DbRepository repo, ref User author)
        {
            Joke joke = new Joke
            {
                Title = "Struś",
                Content = "Jak najłatwiej zabić strusia?" +
            "- Przestraszyć go na betonie.",
                Author = author,
                CreatedDate = DateTime.Now
            };
            repo.AddJoke(joke);

            Joke joke2 = new Joke
            {
                Title = "Pies i pustynia",
                Content = "Biegnie pies przez pustynie, widzi drzewo i mówi:" +
            "- Jeśli to znowu fatamorgana to mi pęcherz pęknie!",
                Author = author,
                CreatedDate = DateTime.Now
            };
            repo.AddJoke(joke2);

            Joke joke3 = new Joke
            {
                Title = "O jeżyku",
                Content = "Chodzi sobie jeżyk dookoła beczki, chodzi i chodzi. W pewnym momencie zdenerwował się i krzyczy:" +
            "- Cholera, kiedy ten płot się skończy?.",
                Author = author,
                CreatedDate = DateTime.Now
            };
            repo.AddJoke(joke3);
        }

        static private void AddSomeComment(DbRepository repo, ref Joke joke, ref User author)
        {
            Comment comment = new Comment
            {
                Content = "Dobre! :)",
                Author = author,
                Joke = joke,
                CreatedDate = DateTime.Now
            };
            repo.AddComment(comment);
        }
    }
}
