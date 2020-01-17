using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbConnection;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public JokesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Jokes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokes()
        {
            return await _context.Jokes.ToListAsync();
        }

        // GET: api/Jokes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Joke>> GetJoke(int id)
        {
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
            return await _context.Comments.Where(c => c.JokeFK == id).ToListAsync();
        }

        [HttpGet("random")]
        public async Task<ActionResult<Joke>> GetRandomJoke()
        {
            //return await _context.Jokes.FindAsync(1);
            return NotFound();
        }

        [HttpGet("search/{title}")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetAllJokesAbout(string title)
        {
            return await _context.Jokes.Where(j => j.Title.ToLower().Contains(title.ToLower())).ToListAsync(); 
        }

        // PUT: api/Jokes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJoke(int id, Joke joke)
        {
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
            _context.Jokes.Add(joke);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJoke", new { id = joke.Id }, joke);
        }

        // DELETE: api/Jokes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Joke>> DeleteJoke(int id)
        {
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
