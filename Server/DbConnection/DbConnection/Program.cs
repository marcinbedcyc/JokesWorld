using System;

namespace DbConnection
{
    public class Program
    {
        public static void Main()
        {
            AddSomeUsers();
            User maricnbedcyc = DbRepository.GetUserByNicknameAndPassword("marcinbedcyc", "password");
            User wlepciax = DbRepository.GetUserByNicknameAndPassword("wlepciax", "password");
            Console.WriteLine(maricnbedcyc.Id);
            Console.WriteLine(wlepciax.Id);
            AddSomeJokes(ref maricnbedcyc);
            AddSomeJokes2(ref wlepciax);
            Joke joke = DbRepository.GetRandomJoke();
            AddSomeComment(ref joke, ref maricnbedcyc);

            //foreach (Joke j in repo.GetJokesAbout("Blondynk"))
            //foreach (Joke j in repo.GetJokesAbout("b"))
            //Console.WriteLine(maricnbedcyc.Jokes);
            //maricnbedcyc.Email = "cmartindev@gmail.com";
            if(DbRepository.DeleteUser(wlepciax.Id))
                Console.WriteLine("Usunięto!\n\n");
            foreach (Joke j in DbRepository.GetAllJokes())
            {
                Console.WriteLine($"Tytuł: {j.Title}");
                Console.WriteLine($"Autor: {j.Author.Nickname}");
                Console.WriteLine(j.Content);
                Console.WriteLine("\n\n");
            }

            foreach (Comment c in DbRepository.GetUsersComments(maricnbedcyc.Id))
            {
                Console.WriteLine($"Autor: {c.Author.Nickname}");
                Console.WriteLine($"Dowcip: {c.Joke.Title}");
                Console.WriteLine(c.Content);
                Console.WriteLine("\n\n");
            }

            foreach (Comment c in DbRepository.GetJokesComments(1))
            {
                Console.WriteLine($"Autor: {c.Author.Nickname}");
                Console.WriteLine($"Dowcip: {c.Joke.Title}");
                Console.WriteLine(c.Content);
                Console.WriteLine("\n\n");
            }

        }

        static private void AddSomeUsers()
        {
            User user = new User
            {
                Name = "Marcin",
                Surname = "Cyc",
                Email = "marcinbedcyc@gmail.com",
                Nickname = "marcinbedcyc",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
            DbRepository.AddUser(user);

            User user1 = new User
            {
                Name = "Kornelia",
                Surname = "Nalepa",
                Email = "wlpeciax@gmail.com",
                Nickname = "wlepciax",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
            DbRepository.AddUser(user1);
        }

        static private void AddSomeJokes( ref User author)
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
            DbRepository.AddJoke(joke);

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
            DbRepository.AddJoke(joke2);

            Joke joke3 = new Joke
            {
                Title = "2Żydów i Watykan",
                Content = "Dwóch Żydów zwiedza Watykan, podziwiają przepych i bogactwo. Jeden mówi: " +
            "-Popatrz, a zaczynali od stajenki..",
                Author = author,
                CreatedDate = DateTime.Now
            };
            DbRepository.AddJoke(joke3);
        }

        static private void AddSomeJokes2( ref User author)
        {
            Joke joke = new Joke
            {
                Title = "Struś",
                Content = "Jak najłatwiej zabić strusia?" +
            "- Przestraszyć go na betonie.",
                Author = author,
                CreatedDate = DateTime.Now
            };
            DbRepository.AddJoke(joke);

            Joke joke2 = new Joke
            {
                Title = "Pies i pustynia",
                Content = "Biegnie pies przez pustynie, widzi drzewo i mówi:" +
            "- Jeśli to znowu fatamorgana to mi pęcherz pęknie!",
                Author = author,
                CreatedDate = DateTime.Now
            };
            DbRepository.AddJoke(joke2);

            Joke joke3 = new Joke
            {
                Title = "O jeżyku",
                Content = "Chodzi sobie jeżyk dookoła beczki, chodzi i chodzi. W pewnym momencie zdenerwował się i krzyczy:" +
            "- Cholera, kiedy ten płot się skończy?.",
                Author = author,
                CreatedDate = DateTime.Now
            };
            DbRepository.AddJoke(joke3);
        }

        static private void AddSomeComment( ref Joke joke, ref User author)
        {
            Comment comment = new Comment
            {
                Content = "Dobre! :)",
                Author = author,
                Joke = joke,
                CreatedDate = DateTime.Now
            };
            DbRepository.AddComment(comment);
        }
    }
}
