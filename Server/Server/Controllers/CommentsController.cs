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
    /// CommentsController. Configuration of urls.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
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

        public CommentsController(MyDbContext context)
        {
            _context = context;
            jokesWorldLog = new EventLog
            {
                Source = source
            };
        }

        /// <summary>
        /// GET: api/comments - get all comments from database and log information to LogEvent.
        /// </summary>
        /// <returns>All comments from database as JSON.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            jokesWorldLog.WriteEntry(string.Format("Get all comments from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.ToListAsync();
        }

        /// <summary>
        /// GET: api/comments/last_ones - get ten recently added comments and log information to LogEvent.
        /// </summary>
        /// <returns>Ten recently added comments as JSON.</returns>
        [HttpGet("last_ones")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetLastComments()
        {
            jokesWorldLog.WriteEntry(string.Format("Get last comments from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            var comments = await _context.Comments.OrderByDescending(c => c.CreatedDate).ToListAsync();

            if (comments.Count > 10)
                return comments.Take(10).ToList();
            else
                return comments.Take(comments.Count).ToList();
        }

        /// <summary>
        /// GET: api/comments/search/{text} - get matching comments to searching text and log informataion to logEvent. 
        /// </summary>
        /// <param name="text">Searching text.</param>
        /// <returns>Matching comments to searching text as JSON.</returns>
        [HttpGet("search/{text}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllJokesAbout(string text)
        {
            jokesWorldLog.WriteEntry(string.Format("Search comments with text: {0} from remote IP addres: {1}", text, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.Where(j => j.Content.ToLower().Contains(text.ToLower())).ToListAsync();
        }

        /// <summary>
        /// GET: api/comments/{id} - get single comment with id passed in parameter and log information to logEvent.
        /// </summary>
        /// <param name="id">Comment's id.</param>
        /// <returns>Comment with id passed in parameter if found as JSON else NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Get comment with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        /// <summary>
        /// PUT: /api/comments/id - edit comment with id passed in parameter.
        /// </summary>
        /// <param name="id">Comment's id to edit.</param>
        /// <param name="comment">New values for comment</param>
        /// <returns>No Content if everything ok. NotFound if comment with passed id doesn't exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            jokesWorldLog.WriteEntry(string.Format("Edit comment with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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
        /// POST: /api/comments - add new comment to databse.
        /// </summary>
        /// <param name="comment">Values for new comments.</param>
        /// <returns>Newly added comment as JSON.</returns>
        /// <seealso cref="CommentsController.GetComment"/>
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            jokesWorldLog.WriteEntry(string.Format("Add comment from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        /// <summary>
        /// DELETE: /api/comments/{id} - delete from database comment with id passed in parameter.
        /// </summary>
        /// <param name="id">Comment's id</param>
        /// <returns>NotFound if comment with passed id doesn't exist else removed comment as JSON.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            jokesWorldLog.WriteEntry(string.Format("Delete comment with id: {0} from remote IP addres: {1}", id, Request.HttpContext.Connection.RemoteIpAddress));
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        /// <summary>
        /// Looking for comment with id passed in parameter in database.
        /// </summary>
        /// <param name="id">Comment's id.</param>
        /// <returns>True if comment with passed id exists in database else false.</returns>
        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
