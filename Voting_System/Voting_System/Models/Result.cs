using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voting_System.Models
{
    public class Result
    {
        public Election Election { get; set; }
        public Issue[] Passed_Issues { get; set; }
        public Candidate Winner { get; set; }
        public Voter[] Voters { get; set; }



        public Result()
        {

        }

        public Result(Election election, Issue[] issues, Candidate winner, Voter[] voters)
        {
            Election = election;
            Passed_Issues = issues;
            Winner = winner;
            Voters = voters;
        }

    }

}
