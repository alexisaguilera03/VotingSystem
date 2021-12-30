using System;

namespace Voting_System.Models
{
    public class Ballot
    {
        public Voter Voter { get; set; }
        public Election Current_Election { get; set; }
        public Issue[] Issues { get; set; }
        public Candidate[] Candidates { get; set; }
        public bool Voter_Status { get; set; }
        public int[] Issues_Results { get; set; }
        public int[] Candidate_Results { get; set; }


        public Ballot()
        {

        }

        public Ballot(Voter voter, Election election, Issue[] issues, Candidate[] candidates, bool status, int[] issuesResults, int[] candidateResults)
        {
            Voter = voter;
            Current_Election = election;
            Issues = issues;
            Candidates = candidates;
            Voter_Status = status;
            Issues_Results = issuesResults;
            Candidate_Results = candidateResults;
        }

    }
}