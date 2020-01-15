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

        [HttpGet("test")]
        public void Test(string nickname)
        {
            DbConnection.Program.Main();
        }

        [HttpGet("nickname/{nickname}")]
        public UserDAO GetUserByNickname(string nickname)
        {
            return DbRepository.GetUserByNickname(nickname).ToDAO();
        }

        [HttpGet("{id}/comments")]
        public List<CommentDAO> GetUsersComment(int id)
        {
            List<CommentDAO> comments = new List<CommentDAO>();
            foreach (Comment c in DbRepository.GetUsersComments(id))
                comments.Add(c.ToDAO());
            return comments;
        }

        [HttpGet("{id}/jokes")]
        public List<JokeDAO> GetUsersJokes(int id)
        {
            List<JokeDAO> jokes = new List<JokeDAO>();
            foreach (Joke j in DbRepository.GetUsersJokes(id))
                jokes.Add(j.ToDAO());
            return jokes;
        }

        [HttpGet("{id}")]
        public UserDAO GetUserById(int id)
        {
            return DbRepository.GetUserById(id).ToDAO();
        }

        [HttpGet]
        public List<UserDAO> GetAllUsers()
        {
            List<UserDAO> users = new List<UserDAO>();
            foreach (User u in DbRepository.GetAllUsers())
                users.Add(u.ToDAO());
            return users;
        }

        [HttpDelete("{id}")]
        public string DeleteUser(int id)
        {
            if (DbRepository.DeleteUser(id))
                return $"User id: {id} deleted";
            else
                return $"User id: {id} doesn't exist";
        }

        [HttpPut("{id}")]
        public string UpdateUser(int id, [FromBody]UserDAO user)
        {
            user.Id = id;
            if (DbRepository.UpdateUser(user.ToModel()))
                return $"User id: {id} updated";
            else
                return $"User id: {id} doesn't exist";
        }

        [HttpPost]
        public string CreateUser([FromBody]UserDAO user)
        {
            if (DbRepository.AddUser(user.ToModel()))
                return "User created";
            else
                return "Some errors";
        }
    }
}
