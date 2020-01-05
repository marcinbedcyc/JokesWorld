using System;

namespace DbConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            DbRepository repo = new DbRepository();
            addSomeUsers(repo);
            User maricnbedcyc = repo.GetUserWithNicknameAndPassword("marcinbedcyc", "password");
            User wlepciax = repo.GetUserWithNicknameAndPassword("wlepciax", "password");
            addSomeJokes(repo, ref maricnbedcyc);
            addSomeJokes2(repo, ref wlepciax);
            Joke joke = repo.GetRandomJoke();
            addSomeComment(repo, ref joke, ref maricnbedcyc);


            //foreach (Joke j in repo.GetJokesAbout("Blondynk"))
            //foreach (Joke j in repo.GetJokesAbout("b"))
            //Console.WriteLine(maricnbedcyc.Jokes);
            foreach (Joke j in repo.GetUsersJokes(maricnbedcyc.Id))
            {
                Console.WriteLine($"Tytuł: {j.Title}");
                Console.WriteLine($"Autor: {j.Author.Nickname}");
                Console.WriteLine(j.Content);
                Console.WriteLine("\n\n");
            }

            foreach (Comment c in maricnbedcyc.Comments)
            {
                Console.WriteLine($"Autor: {c.Author.Nickname}");
                Console.WriteLine($"Dowcip: {c.Joke.Title}");
                Console.WriteLine(c.Content);
                Console.WriteLine("\n\n");
            }

        }

        static private void addSomeUsers(DbRepository repo)
        {
            User user = new User();
            user.Name = "Marcin";
            user.Surname = "Cyc";
            user.Email = "marcinbedcyc@gmail.com";
            user.Nickname = "marcinbedcyc";
            user.Password = BCrypt.Net.BCrypt.HashPassword("password");
            repo.AddUser(user);

            User user1 = new User();
            user1.Name = "Kornelia";
            user1.Surname = "Nalepa";
            user1.Email = "wlpeciax@gmail.com";
            user1.Nickname = "wlepciax";
            user1.Password = BCrypt.Net.BCrypt.HashPassword("password");
            repo.AddUser(user1);
        }

        static private void addSomeJokes(DbRepository repo, ref User author)
        {
            Joke joke = new Joke();
            joke.Title = "2Blondynki i nagroda";
            joke.Content = "Dwie blondynki wygrały nagrodę. Nie wiedzą co z nią zrobić, aż w końcu Zuza mówi:" +
            "-wiem, polecimy na słońce" +
            "- co Ty spalimy się" +
            "- przecierz polecimy w nocy.";
            joke.Author = author;
            joke.CreatedDate = DateTime.Now;
            repo.AddJoke(joke);

            Joke joke2 = new Joke();
            joke2.Title = "2Blondynki i mecz";
            joke2.Content = "Dwie blondynki oglądają mecz Brazylia vs Czechy,pierwsza poszla zrobic popcorn...druga krzyczy" +
            "- GOOOL" +
            "- kto strzelił ? " +
            "- Ronaldinho" +
            "za chwile" +
            "- jest drugi gol" +
            "- kto szczelił" +
            "- jakis Replay.";
            joke2.Author = author;
            joke2.CreatedDate = DateTime.Now;
            repo.AddJoke(joke2);

            Joke joke3 = new Joke();
            joke3.Title = "2Żydów i Watykan";
            joke3.Content = "Dwóch Żydów zwiedza Watykan, podziwiają przepych i bogactwo. Jeden mówi: " +
            "-Popatrz, a zaczynali od stajenki..";
            joke3.Author = author;
            joke3.CreatedDate = DateTime.Now;
            repo.AddJoke(joke3);
        }

        static private void addSomeJokes2(DbRepository repo, ref User author)
        {
            Joke joke = new Joke();
            joke.Title = "Struś";
            joke.Content = "Jak najłatwiej zabić strusia?" +
            "- Przestraszyć go na betonie.";
            joke.Author = author;
            joke.CreatedDate = DateTime.Now;
            repo.AddJoke(joke);

            Joke joke2 = new Joke();
            joke2.Title = "Pies i pustynia";
            joke2.Content = "Biegnie pies przez pustynie, widzi drzewo i mówi:" +
            "- Jeśli to znowu fatamorgana to mi pęcherz pęknie!";
            joke2.Author = author;
            joke2.CreatedDate = DateTime.Now;
            repo.AddJoke(joke2);

            Joke joke3 = new Joke();
            joke3.Title = "O jeżyku";
            joke3.Content = "Chodzi sobie jeżyk dookoła beczki, chodzi i chodzi. W pewnym momencie zdenerwował się i krzyczy:" +
            "- Cholera, kiedy ten płot się skończy?.";
            joke3.Author = author;
            joke3.CreatedDate = DateTime.Now;
            repo.AddJoke(joke3);
        }

        static private void addSomeComment(DbRepository repo, ref Joke joke, ref User author)
        {
            Comment comment = new Comment();
            comment.Content = "Dobre! :)";
            comment.Author = author;
            comment.Joke = joke;
            comment.CreatedDate = DateTime.Now;
            repo.AddComment(comment);
        }
    }
}
