using System.Diagnostics;

namespace Voting_System.Models
{
    public class Candidate
    {
        public int canidate_Id { get; }
        public string First_Name { get; }
        public string Last_Name { get; }
        public string Affiliation { get; set; }
        public int Assigned_Election { get; set; }
        public int Votes { get; set; }

        public Candidate()
        {

        }

        public Candidate(int canidate_ID, string first_Name, string last_Name, string affiliation, int assignedElection, int votes=0)
        {
            this.canidate_Id = canidate_ID;
            this.First_Name = first_Name;
            this.Last_Name = last_Name;
            this.Affiliation = affiliation;
            this.Assigned_Election = assignedElection;
            this.Votes = votes;
        }
    }
}
