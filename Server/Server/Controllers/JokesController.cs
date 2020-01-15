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
    public class JokesController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;

        public JokesController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}/comments")]
        public List<CommentDAO> GetJokesComment(int id)
        {
            List<CommentDAO> comments = new List<CommentDAO>();
            foreach (Comment c in DbRepository.GetJokesComments(id))
                comments.Add(c.ToDAO());
            return comments;
        }


        [HttpGet("{id}")]
        public JokeDAO GetJokeById(int id)
        {
            return DbRepository.GetJokeById(id).ToDAO();
        }

        [HttpGet]
        public List<JokeDAO> GetAllJokes()
        {
            List<JokeDAO> jokes = new List<JokeDAO>();
            foreach (Joke j in DbRepository.GetAllJokes())
                jokes.Add(j.ToDAO());
            return jokes;
        }

        [HttpGet("search/{title}")]
        public List<JokeDAO> FindAllJokesAbout(string title)
        {
            List<JokeDAO> jokes = new List<JokeDAO>();
            foreach (Joke j in DbRepository.GetJokesAbout(title))
                jokes.Add(j.ToDAO());
            return jokes;
        }

        [HttpGet("random")]
        public JokeDAO GetRandomJoke()
        {
            return DbRepository.GetRandomJoke().ToDAO();
        }

        [HttpDelete("{id}")]
        public string DeleteJoke(int id)
        {
            if (DbRepository.DeleteJoke(id))
                return $"Joke id: {id} deleted";
            else
                return $"Joke id: {id} doesn't exist";
        }

        [HttpPut("{id}")]
        public string UpdateJoke(int id, [FromBody]JokeDAO joke)
        {
            joke.Id = id;
            if (DbRepository.UpdateJoke(joke.ToModel()))
                return $"Joke id: {id} updated";
            else
                return $"Joke id: {id} doesn't exist";
        }

        [HttpPost]
        public string CreateJoke([FromBody]JokeDAO joke)
        {
            if (DbRepository.AddJoke(joke.ToModel()))
                return "Joke created";
            else
                return "Some errors";
        }
    }
}
