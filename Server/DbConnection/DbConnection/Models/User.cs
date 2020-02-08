using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnection
{
    /// <summary>
    /// Class represents user's entity in db.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User's id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// User's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User's surname.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// User's e-mail addres.
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// User's nickname.
        /// </summary>
        [Required]
        public string Nickname { get; set; }
        /// <summary>
        /// User's hashed password.
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Collection with all user's comments.
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
        /// <summary>
        /// Collection with all user's jokes.
        /// </summary>
        public ICollection<Joke> Jokes { get; set; }
    }
}
