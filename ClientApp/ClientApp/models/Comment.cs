using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.models
{
    /// <summary>
    /// Class reprents comment object.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Id from db.
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Comment's content.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Comment's creation date.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Comment'a author id in db.
        /// </summary>
        public int? AuthorFK { get; set; }
        /// <summary>
        /// Comment's joke id in db.
        /// </summary>
        public int? JokeFK { get; set; }
    }
}
