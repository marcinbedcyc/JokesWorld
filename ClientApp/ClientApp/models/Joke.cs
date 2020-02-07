using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.models
{
    /// <summary>
    /// Class represents joke object.
    /// </summary>
    public class Joke
    {
        /// <summary>
        /// Id from db.
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Joke's title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Joke's creation date.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Joke's content.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Author id from db.
        /// </summary>
        public int? AuthorFK { get; set; }

        public override string ToString()
        {
            return this.Id + " " + this.Title + " " + this.Content + " " +  this.CreatedDate.ToString();
        }
    }

}
