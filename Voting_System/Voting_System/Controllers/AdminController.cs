using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Voting_System.Accessors;
using Voting_System.Models;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace Voting_System.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPut]
        public int ChangeElection(int election_id) 
        {
            AdminAccessor admin = new AdminAccessor(_configuration);
            return admin.ChangeActiveElection(election_id);
        }
        [HttpPost]
        public int AddElection(int year) 
        {
            AdminAccessor admin = new AdminAccessor(_configuration);
            return admin.AddElection(year);
        }
        [HttpPost]
        public int AddCandidate(string first_name, string last_name, string affiliation, int election_id)
        {
            AdminAccessor admin = new AdminAccessor(_configuration);
            return admin.AddCandidate(first_name, last_name, affiliation, election_id);
        }
        [HttpPost]
        public int AddIssue(string issue_description, string issue_title, int election_id)
        {
            AdminAccessor admin = new AdminAccessor(_configuration);
            return admin.AddIssue(issue_description, issue_title, election_id);
        }
    }
}
