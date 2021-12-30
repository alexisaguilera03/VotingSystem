using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class ResultsController : ControllerBase
    {
        private  IConfiguration _configuration;

        public ResultsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public Result Get(int id=-1, int year=-1) 
        {
            Election currentElection = new Election();
            VoterAccessor votersAccessor = new VoterAccessor(_configuration);
            IssueAccessor issueAccessor = new IssueAccessor(_configuration);
            CandidatesAccessor candidateAccessor = new CandidatesAccessor(_configuration);
            ResultsAccessor resultsAccessor = new ResultsAccessor(_configuration);
            try
            {
                ElectionAccessor electionAccessor = new ElectionAccessor(_configuration);
                 currentElection = electionAccessor.GetElection(id,year);

            }catch(ArgumentException exception)
            {
                System.Console.WriteLine(exception.Message);
                //raise another error to have web page display error page?
            }

            Issue[] issues = issueAccessor.GetIssues(currentElection.electionId);
            Candidate[] candidates = candidateAccessor.GetCandidates(currentElection.electionId);
            Voter[] voters = votersAccessor.GetVoters(currentElection.electionId);
            List <Issue> passedIssues = new List<Issue>();
            int totalVoters = voters.Length;
            double percentage = totalVoters * .5;
            getCandidateVotes(ref candidates, resultsAccessor);
            getIssueVotes(ref issues, resultsAccessor);
            issuePassed(ref issues, percentage, ref passedIssues);
            Candidate winner = getWinner(candidates);
            

            Result result = new Result(currentElection, passedIssues.ToArray(), winner, voters);
            return result;
        }

        [HttpGet]
        public Voter[] GetVoters(int electionId)
        {
            VoterAccessor voterAccessor = new VoterAccessor(_configuration);
            return voterAccessor.GetVoters(electionId);
        }


        private void getCandidateVotes(ref Candidate[] candidates, ResultsAccessor accessor)
        {
            for (var i = 0; i < candidates.Length; i++)
            {
                accessor.getCandidateVotes(ref candidates[i]);
            }

            return;
        }

        private void getIssueVotes(ref Issue[] issues, ResultsAccessor accessor)
        {
            for (var i = 0; i < issues.Length; i++)
            {
                accessor.getIssueVotes(ref issues[i]);
            }

            return;
        }

        private void issuePassed(ref Issue[] issues, double percentage, ref List<Issue> passedIssues)
        {
            foreach (var issue in issues)
            {
                if (issue.Votes > percentage)
                {
                    issue.Passed = true;
                    passedIssues.Add((issue));
                }
            }

            return;
        }

        private Candidate getWinner(Candidate[] candidates)
        {
            Candidate winner = new Candidate();
            int max = 0;
            foreach (var candidate in candidates)
            {
                if (candidate.Votes > max)
                {
                    winner = candidate;
                    max = candidate.Votes;
                }
            }

            return winner;
        }
    }
}
