using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnection.ORMs
{
    public class UserORM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }

        public User ToModel()
        {
            User user = new User
            {
                Id = this.Id,
                Name = this.Name,
                Surname = this.Surname,
                Email = this.Email,
                Nickname = this.Nickname,
                Password = this.Password,
                Jokes = new DbRepository().GetUsersJokes(this.Id),
                Comments = new DbRepository().GetUsersComments(this.Id)
            };
            return user;
        }
    }
}
