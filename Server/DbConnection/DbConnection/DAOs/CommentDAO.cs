using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnection.DAOs
{
    public class CommentDAO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserDAO Author { get; set; }
        public int AuthorFK { get; set; }
        public JokeDAO Joke { get; set; }
        public int JokeFK { get; set; }

        public Comment ToModel()
        {
            Comment comment = new Comment
            {
                Id = this.Id,
                Content = this.Content,
                CreatedDate = this.CreatedDate,
                Author = this.Author.ToModel(),
                AuthorFK = this.AuthorFK,
                Joke = this.Joke.ToModel(),
                JokeFK = this.JokeFK
            };
            return comment;
        }
    }
}
