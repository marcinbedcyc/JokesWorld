using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnection;
using DbConnection.DAOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
       

        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("nickname/{nickname}")]
        public UserDAO GetUserByNickname(string nickname)
        {
            return DbRepository.GetUserByNickname(nickname).ToDAO();
        }

        [HttpGet("id/{id}/comments")]
        public List<CommentDAO> GetUsersComment(int id)
        {
            List<CommentDAO> comments = new List<CommentDAO>();
            foreach (Comment c in DbRepository.GetUsersComments(id))
                comments.Add(c.ToDAO());
            return comments;
        }

        [HttpGet("id/{id}/jokes")]
        public List<JokeDAO> GetUsersJokes(int id)
        {
            List<JokeDAO> jokes = new List<JokeDAO>();
            foreach (Joke j in DbRepository.GetUsersJokes(id))
                jokes.Add(j.ToDAO());
            return jokes;
        }

        [HttpGet("id/{id}")]
        public UserDAO GetUserByNickname(int id)
        {
            return DbRepository.GetUserById(id).ToDAO();
        }

        //[HttpGet("{nickname}/{password}")]
        //public UserDAO GetUserByNicknameAndPassword(string nickname, string password)
        //{
        //    return DbRepository.GetUserByNicknameAndPassword(nickname, password).ToDAO();
        //}

        [HttpGet]
        public List<UserDAO> GetAllUsers()
        {
            List<UserDAO> users = new List<UserDAO>();
            foreach (User u in DbRepository.GetAllUsers())
                users.Add(u.ToDAO());
            return users;
        }
    }
}
