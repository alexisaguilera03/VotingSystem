
using System;
using Voting_System.Accessors;

namespace Voting_System.Models
{
    public class Election
    {
        public int year { get; set; } 
        public int electionId { get; set; }
        public bool active { get; set; }

        
        public Election(int electionId, int year, bool active)
        {
            this.year = year;
            this.electionId = electionId;
            this.active = active;
        }

        public Election()
        {
        }
    }
}
