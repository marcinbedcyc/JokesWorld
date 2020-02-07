using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbConnection;
using System.Diagnostics;
using System.IO;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly string source = "JokesWorldSource";
        private readonly EventLog jokesWorldLog;

        public JokesController(MyDbContext context)
        {
            _context = context;
            jokesWorldLog = new EventLog
            {
                Source = source
            };
        }

        // GET: api/Jokes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokes()
        {
            jokesWorldLog.WriteEntry(string.Format("Get all jokes from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Jokes.ToListAsync();
        }

        [HttpGet("process_dir")]
        public string Get1()
        {
            return Process.GetCurrentProcess().MainModule.FileName.ToString();
        }

        [HttpGet("current_dir")]
        public string Get2()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName).ToString().Replace(@"\", @"\\");
        }

        [HttpGet("last_ones")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetLastJokes()
        {
            jokesWorldLog.WriteEntry(string.Format("Get last jokes from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            var jokes = await _context.Jokes.OrderByDescending(j => j.CreatedDate).ToListAsync();
            
            if(jokes.Count > 10)
                return jokes.Take(10).ToList();
            else
                return jokes.Take(jokes.Count).ToList();
        }

        // GET: api/Jokes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Joke>> GetJoke(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get joke with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var joke = await _context.Jokes.FindAsync(id);

            if (joke == null)
            {
                return NotFound();
            }

            return joke;
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetJokesComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get joke's (with id: {0}) comments from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.Where(c => c.JokeFK == id).ToListAsync();
        }

        //[HttpGet("random")]
        //public async Task<ActionResult<Joke>> GetRandomJoke()
        //{
        //    //return await _context.Jokes.FindAsync(1);
        //    return NotFound();
        //}

        [HttpGet("search/{title}")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetAllJokesAbout(string title)
        {
            jokesWorldLog.WriteEntry(string.Format("Search jokes with title: {0} from remote IP addres: {1}", title, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Jokes.Where(j => j.Title.ToLower().Contains(title.ToLower())).ToListAsync(); 
        }

        // PUT: api/Jokes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJoke(int id, Joke joke)
        {
            jokesWorldLog.WriteEntry(string.Format("Edit joke with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            if (id != joke.Id)
            {
                return BadRequest();
            }

            _context.Entry(joke).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JokeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jokes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Joke>> PostJoke(Joke joke)
        {
            jokesWorldLog.WriteEntry(string.Format("Add new joke from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            _context.Jokes.Add(joke);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJoke", new { id = joke.Id }, joke);
        }

        // DELETE: api/Jokes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Joke>> DeleteJoke(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Delete joke with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var joke = await _context.Jokes.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }

            _context.Jokes.Remove(joke);
            await _context.SaveChangesAsync();

            return joke;
        }

        private bool JokeExists(int id)
        {
            return _context.Jokes.Any(e => e.Id == id);
        }
    }
}
