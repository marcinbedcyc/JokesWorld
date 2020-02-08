using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnection
{
    /// <summary>
    /// Class represents comment's entity in db.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Comment's id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Comment's content.
        /// </summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// Comment's creation date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Comment's author.
        /// </summary>
        public User Author { get; set; }
        /// <summary>
        /// Commet's author's id.
        /// </summary>
        public int AuthorFK { get; set; }

        /// <summary>
        /// Comment's joke.
        /// </summary>
        public Joke Joke { get; set; }
        /// <summary>
        /// Comment's joke's id.
        /// </summary>
        public int JokeFK { get; set; }
    }
}
