using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnection.DAOs
{
    public class JokeDAO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public UserDAO Author { get; set; }
        public int AuthorFK { get; set; }

        public Joke ToModel()
        {
            Joke joke = new Joke
            {
                Id = this.Id,
                Title = this.Title,
                CreatedDate = this.CreatedDate,
                Content = this.Content,
                Author = this.Author.ToModel(),
                AuthorFK = this.AuthorFK,
                Comments = DbRepository.GetJokesComments(this.Id)
            };
            return joke;
        }

    }
}
