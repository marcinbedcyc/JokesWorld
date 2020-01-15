using DbConnection.DAOs;
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

        public CommentDAO ToDAO()
        {
            CommentDAO commentDAO = new CommentDAO
            {
                Id = this.Id,
                Content = this.Content,
                CreatedDate = this.CreatedDate,
                AuthorFK = this.AuthorFK,
                JokeFK = this.JokeFK
            };
            return commentDAO;
        }
    }
}
