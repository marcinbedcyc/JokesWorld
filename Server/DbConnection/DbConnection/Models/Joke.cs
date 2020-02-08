using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnection
{
    /// <summary>
    /// Class represents joke's entity in db.
    /// </summary>
    public class Joke
    {
        /// <summary>
        /// Joke's id in db.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Joke's title.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Joke's creation date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Joke's content.
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Joke's author.
        /// </summary>
        public User Author { get; set; }
        /// <summary>
        /// Joke's author's id.
        /// </summary>
        public int AuthorFK { get; set; }

        /// <summary>
        /// Collection with all joke's comment.
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
    }
}
