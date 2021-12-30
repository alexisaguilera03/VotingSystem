using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public Voter Login(string username = null, string password = null)

        {
            if (username == "undefined" || password == "undefined" || username == null || password == null)
            {
                return validate(username, password);
            }else if (username == "" || password == "" || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return validate(username, password);
            }

            VoterAccessor accessor = new VoterAccessor(_configuration);
            var voter = accessor.getVoter(username);
            voter = (voter.Password == password) ? voter : new Voter(-1, "null", "null", "null", "incorrect", "incorrect"); 
            return voter;

        }
        [HttpGet]
        public bool isEmpty(string value) //I am fairly certain this function is not used. todo: delete?
        {
            return string.IsNullOrWhiteSpace(value);
        }

        private Voter validate(string username, string password)
        //in this function, checking for null or white space and also checking for undefined is needed because if a user doesn't
        //enter anything then the values are "undefined" but if the user deletes their input and leaves it empty it is null
        {
            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
            {
                return new Voter(-1, "null", "null", "null", "null", "null");
            }
            else if (string.IsNullOrWhiteSpace(username))
            {
                return new Voter(-1, "null", "null", "null", "missing", "null");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                return new Voter(-1, "null", "null", "null", "null", "missing");
            }
            //else if(username == "undefined " && password == "undefined")
            else if (username == "undefined")
            {
                return new Voter(-1, "null", "null", "null", "missing", "null");
            }
            else if (password == "undefined")
            {
                return new Voter(-1, "null", "null", "null", "null", "missing");
            }

            return new Voter(-1, "null", "null", "null", "missing", "missing");
        }
    }
}
