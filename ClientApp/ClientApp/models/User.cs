using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.models
{
    /// <summary>
    /// Class represents user object.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id from db.
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// User's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User's surname.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// User's e-mail.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User's nickname.
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// Hashed user's password.
        /// </summary>
        public string Password { get; set; }
    }
}
