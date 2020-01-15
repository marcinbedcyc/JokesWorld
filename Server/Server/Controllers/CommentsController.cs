using DbConnection;
using DbConnection.DAOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;

        public CommentsController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public CommentDAO GetCommentById(int id)
        {
            return DbRepository.GetCommentById(id).ToDAO();
        }

        [HttpGet]
        public List<CommentDAO> GetAllComments()
        {
            List<CommentDAO> comments = new List<CommentDAO>();
            foreach (Comment c in DbRepository.GetAllComments())
                comments.Add(c.ToDAO());
            return comments;
        }

        [HttpDelete("{id}")]
        public string DeleteComment(int id)
        {
            if (DbRepository.DeleteComment(id))
                return $"Comment id: {id} deleted";
            else
                return $"Comment id: {id} doesn't exist";
        }

        [HttpPut("{id}")]
        public string UpdateComment(int id, [FromBody]CommentDAO comment)
        {
            comment.Id = id;
            if (DbRepository.UpdateComment(comment.ToModel()))
                return $"Comment id: {id} updated";
            else
                return $"Comment id: {id} doesn't exist";
        }

        [HttpPost]
        public string CreateComment([FromBody]CommentDAO comment)
        {
            if (DbRepository.AddComment(comment.ToModel()))
                return "Comment created";
            else
                return "Some errors";
        }
    }
}
