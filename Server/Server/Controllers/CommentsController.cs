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
    public class CommentsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly string source = "JokesWorldSource";
        private readonly EventLog jokesWorldLog;

        public CommentsController(MyDbContext context)
        {
            _context = context;
            jokesWorldLog = new EventLog
            {
                Source = source
            };
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            jokesWorldLog.WriteEntry(string.Format("Get all comments from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.ToListAsync();
        }

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

        [HttpGet("search/{text}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllJokesAbout(string text)
        {
            jokesWorldLog.WriteEntry(string.Format("Search comments with text: {0} from remote IP addres: {1}", text, Request.HttpContext.Connection.RemoteIpAddress));
            return await _context.Comments.Where(j => j.Content.ToLower().Contains(text.ToLower())).ToListAsync();
        }

        // GET: api/Comments/5
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

        // PUT: api/Comments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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

        // POST: api/Comments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            jokesWorldLog.WriteEntry(string.Format("Add comment from remote IP addres: {0}", Request.HttpContext.Connection.RemoteIpAddress));
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
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

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
