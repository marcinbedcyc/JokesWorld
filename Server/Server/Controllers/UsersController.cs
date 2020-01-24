using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbConnection;
using System.Diagnostics;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly string source = "JokesWorldSource";
        private readonly EventLog jokesWorldLog;

        public UsersController(MyDbContext context)
        {
            _context = context;
            jokesWorldLog = new EventLog
            {
                Source = source
            };
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            jokesWorldLog.WriteEntry(string.Format("Get all users from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user with id : {0} from remote IP addres: {1}", id,  Request.HttpContext.Connection.RemoteIpAddress));
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("nickname/{nickname}")]
        public async Task<ActionResult<User>> GetUserByNickname(string nickname)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user with nickname: {0} from remote IP addres: {1}", nickname, Request.HttpContext.Connection.RemoteIpAddress));
            var user = await _context.Users.Where(u => u.Nickname == nickname).SingleAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetUsersComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) comments from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.Where(c => c.AuthorFK == id).ToListAsync();
        }

        [HttpGet("{id}/last_comment")]
        public async Task<ActionResult<Comment>> GetLastComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) last comment from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var comments = await _context.Comments.Where(c => c.AuthorFK == id).ToListAsync();
            if (comments.Count == 0)
            {
                return new Comment();
            }
            return comments.OrderByDescending(c => c.CreatedDate).First();
        }

        [HttpGet("{id}/last_joke")]
        public async Task<ActionResult<Joke>> GetLastJoke(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) last joke from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var jokes = await _context.Jokes.Where(j => j.AuthorFK == id).ToListAsync();
            if (jokes.Count == 0)
            {
                return new Joke();
            }
            return jokes.OrderByDescending(j => j.CreatedDate).First();
        }

        [HttpGet("{id}/jokes")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetUsersJokes(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) jokes from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Jokes.Where(j => j.AuthorFK == id).ToListAsync();
        }

        [HttpGet("search/{text}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAbout(string text)
        {
            jokesWorldLog.WriteEntry(string.Format("Search user with text: {0} from remote IP addres: {1}", text, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Users.Where(u => (u.Nickname + u.Name + u.Surname + u.Email).ToLower().Contains(text.ToLower())).ToListAsync();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            jokesWorldLog.WriteEntry(string.Format("Add new user from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Delete user with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
