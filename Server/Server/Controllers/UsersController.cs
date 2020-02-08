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
    /// <summary>
    /// UsersController. Configuration of urls.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
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

        public UsersController(MyDbContext context)
        {
            _context = context;
            jokesWorldLog = new EventLog
            {
                Source = source
            };
        }

        /// <summary>
        /// GET: api/users - get all available users in database.
        /// </summary>
        /// <returns>Enumerable collection with users as JSON.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            jokesWorldLog.WriteEntry(string.Format("Get all users from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// GET: api/users/{id} - get user with id passed in parameter.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>User with id passed in parameter if found as JSON else NotFound.</returns>
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

        /// <summary>
        /// GET: api/users/nickname/{nickname} - get user with nickname passed in parameter.
        /// </summary>
        /// <param name="nickname">User's nickname.</param>
        /// <returns>User with nickname passed in parameter if found as JSON else NotFound.</returns>
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

        /// <summary>
        /// GET: api/users/{id}/comments - get all user's comments in database.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>Enumerable collection with user's comments as JSON.</returns>
        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetUsersComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) comments from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.Where(c => c.AuthorFK == id).ToListAsync();
        }

        /// <summary>
        /// GET: api/users/{id}/last_comment - get recently added comment by user with id passed in parameter.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>Recently added comment by user as JSON or NotFound.</returns>
        [HttpGet("{id}/last_comment")]
        public async Task<ActionResult<Comment>> GetLastComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) last comment from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var comments = await _context.Comments.Where(c => c.AuthorFK == id).ToListAsync();
            if (comments.Count == 0)
            {
                return NotFound();
            }
            return comments.OrderByDescending(c => c.CreatedDate).First();
        }

        /// <summary>
        /// GET: api/users/{id}/last_joke - get recently added joke by user with id passed in parameter.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>Recently added joke by user as JSON or NotFound.</returns>
        [HttpGet("{id}/last_joke")]
        public async Task<ActionResult<Joke>> GetLastJoke(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) last joke from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var jokes = await _context.Jokes.Where(j => j.AuthorFK == id).ToListAsync();
            if (jokes.Count == 0)
            {
                return NotFound();
            }
            return jokes.OrderByDescending(j => j.CreatedDate).First();
        }

        /// <summary>
        /// GET: api/users/{id}/jokes - get all user's jokes in database.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>Enumerable collection with user's jokes as JSON.</returns>
        [HttpGet("{id}/jokes")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetUsersJokes(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get user's (with id: {0}) jokes from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Jokes.Where(j => j.AuthorFK == id).ToListAsync();
        }

        /// <summary>
        /// GET: api/users/search/{text} - get all matching users to searching text passed in parameter.(Searching by name, surname, email and nickname)
        /// </summary>
        /// <param name="text">Searching text.</param>
        /// <returns>Enumerable collection with users matching to searching text as JSON.</returns>
        [HttpGet("search/{text}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAbout(string text)
        {
            jokesWorldLog.WriteEntry(string.Format("Search user with text: {0} from remote IP addres: {1}", text, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Users.Where(u => (u.Nickname + u.Name + u.Surname + u.Email).ToLower().Contains(text.ToLower())).ToListAsync();
        }

        /// <summary>
        /// PUT: api/users/{id} - edit user with id passed in parameter.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <param name="user">New values for user.</param>
        /// <returns>No Content if everything ok. NotFound if user with passed id doesn't exist.</returns>
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

        /// <summary>
        /// POST: api/jokes - add new joke to database.
        /// </summary>
        /// <param name="user">Values for new user.</param>
        /// <returns>Newly added user as JSON.</returns>
        /// <seealso cref="UsersController.GetUser"/>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            jokesWorldLog.WriteEntry(string.Format("Add new user from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// DELETE: api/users - delete user with id passed in parameter.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>NotFound if user with passed id doesn't exist else removed user as JSON.</returns>
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

        /// <summary>
        /// Looking for user with id passed in parameter in database.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>True if user with passed id exists in database else false.</returns>
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
