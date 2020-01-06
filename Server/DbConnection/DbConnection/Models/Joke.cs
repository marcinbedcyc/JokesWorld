using DbConnection.ORMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnection
{
    public class Joke
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Content { get; set; }

        public User Author { get; set; }
        public int AuthorFK { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public JokeORM ToORM()
        {
            JokeORM jokeORM = new JokeORM
            {
                Id = this.Id,
                Title = this.Title,
                Content = this.Content,
                CreatedDate = this.CreatedDate,
                Author = this.Author.ToORM(),
                AuthorFK = this.AuthorFK
            };
            return jokeORM;
        }
    }
}
