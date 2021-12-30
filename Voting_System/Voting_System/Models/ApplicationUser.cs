using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voting_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Voter User { get; set; }
        public string Role { get; set; }

        public ApplicationUser()
        {

        }

        public ApplicationUser(Voter user, string role)
        {
            User = user;
            Role = role;
        }
    }
    
}
