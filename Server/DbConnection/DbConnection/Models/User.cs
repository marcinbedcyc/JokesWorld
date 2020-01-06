using DbConnection.ORMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnection
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        public string Password { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Joke> Jokes { get; set; }

        public UserORM ToORM()
        {
            UserORM userORM = new UserORM
            {
                Id = this.Id,
                Name = this.Name,
                Surname = this.Surname,
                Email = this.Email,
                Password = this.Password,
                Nickname = this.Password
            };
            return userORM;
        }
    }
}
