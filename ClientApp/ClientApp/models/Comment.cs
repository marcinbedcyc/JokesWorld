using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.models
{
    public class Comment
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? AuthorFK { get; set; }
        public int? JokeFK { get; set; }
    }
}
