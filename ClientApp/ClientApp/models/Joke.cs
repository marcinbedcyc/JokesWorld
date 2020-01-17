using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.models
{
    public class Joke
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }

        public int AuthorFK { get; set; }

        public override string ToString()
        {
            return this.Id + " " + this.Title + " " + this.Content + " " +  this.CreatedDate.ToString();
        }
    }

}
