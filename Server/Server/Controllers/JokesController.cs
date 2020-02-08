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
    /// <summary>
    /// JokesController. Configuration of urls.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        /// <summary>
        /// Database's context.
        /// </summary>
        private readonly MyDbContext _context;
        /// <summary>
        /// Name of EventLog's source.
        /// </summary>
        private readonly string source = "JokesWorldSource";
        /// <summary>
        /// EventLog object to log information who and what get from sever.
        /// </summary>
        private readonly EventLog jokesWorldLog;

        public JokesController(MyDbContext context)
        {
            _context = context;
            jokesWorldLog = new EventLog
            {
                Source = source
            };
        }

        /// <summary>
        /// GET: api/jokes - get all available jokes in database.
        /// </summary>
        /// <returns>Enumerable collection with jokes as JSON.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokes()
        {
            jokesWorldLog.WriteEntry(string.Format("Get all jokes from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Jokes.ToListAsync();
        }

        /// <summary>
        /// GET: api/jokes/last_ones - get ten recently added jokes in database.
        /// </summary>
        /// <returns>Enumerable collection with jokes as JSON(up to 10 objects).</returns>
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

        /// <summary>
        /// GET: api/jokes/{id} - get joke with id passed in parameter.
        /// </summary>
        /// <param name="id">Joke's id.</param>
        /// <returns>Joke with id passed in parameter if found as JSON else NotFound.</returns>
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

        /// <summary>
        /// GET: api/jokes/{id}/comments - get joke's (with id passed in parameter) comments.
        /// </summary>
        /// <param name="id">Joke's id.</param>
        /// <returns>Enumerable collection with joke's comments as JSON.</returns>
        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetJokesComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get joke's (with id: {0}) comments from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.Where(c => c.JokeFK == id).ToListAsync();
        }

        /// <summary>
        /// GET: api/jokes/search/{title} - get jokes with title matching to searching title.
        /// </summary>
        /// <param name="title">Searching title.</param>
        /// <returns>Enumerable collection with matching jokes as JSON.</returns>
        [HttpGet("search/{title}")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetAllJokesAbout(string title)
        {
            jokesWorldLog.WriteEntry(string.Format("Search jokes with title: {0} from remote IP addres: {1}", title, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Jokes.Where(j => j.Title.ToLower().Contains(title.ToLower())).ToListAsync(); 
        }

        /// <summary>
        /// PUT: api/jokes/{id} - edit joke with id passed in parameter.
        /// </summary>
        /// <param name="id">Joke's id.</param>
        /// <param name="joke">New values for joke.</param>
        /// <returns>No Content if everything ok. NotFound if joke with passed id doesn't exist.</returns>
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

        /// <summary>
        /// POST: api/jokes - add new joke to database.
        /// </summary>
        /// <param name="joke">Values for new joke.</param>
        /// <returns>Newly added joke as JSON.</returns>
        /// <seealso cref="JokesController.GetJoke"/>
        [HttpPost]
        public async Task<ActionResult<Joke>> PostJoke(Joke joke)
        {
            jokesWorldLog.WriteEntry(string.Format("Add new joke from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            _context.Jokes.Add(joke);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJoke", new { id = joke.Id }, joke);
        }

        /// <summary>
        /// DELETE: api/jokes - delete joke with id passed in parameter.
        /// </summary>
        /// <param name="id">Joke's id.</param>
        /// <returns>NotFound if joke with passed id doesn't exist else removed joke as JSON.</returns>
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

        /// <summary>
        /// Looking for joke with id passed in parameter in database.
        /// </summary>
        /// <param name="id">Joke's id.</param>
        /// <returns>True if joke with passed id exists in database else false.</returns>
        private bool JokeExists(int id)
        {
            return _context.Jokes.Any(e => e.Id == id);
        }
    }
}
