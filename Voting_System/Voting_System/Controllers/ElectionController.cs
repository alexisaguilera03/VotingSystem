
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ElectionController: ControllerBase
    {
        private IConfiguration _configuration;

        public ElectionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public Election[] GetElections()
        {
            ElectionAccessor accessor = new ElectionAccessor(_configuration);
            Election[] elections = accessor.GetAllElections();
            return elections;
        }
    }
}
