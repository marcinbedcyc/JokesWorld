using DbConnection.ORMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnection
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public User Author { get; set; }
        public int AuthorFK { get; set; }

        public Joke Joke { get; set; }
        public int JokeFK { get; set; }

        public CommentORM ToORM()
        {
            CommentORM commentORM = new CommentORM
            {
                Id = this.Id,
                Content = this.Content,
                CreatedDate = this.CreatedDate,
                Author = this.Author.ToORM(),
                AuthorFK = this.AuthorFK,
                Joke = this.Joke.ToORM(),
                JokeFK = this.JokeFK
            };
            return commentORM;
        }
    }
}
