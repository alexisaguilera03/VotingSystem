using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BallotController: ControllerBase
    {

        private readonly IConfiguration _configuration;
        public BallotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public Ballot GetBallot(string username, string password)
        {
            
            Election currentElection = new Election();
            Issue[] issues = Array.Empty<Issue>();
            Candidate[] candidates = Array.Empty<Candidate>();
            Voter voter = new Voter();
            bool status = true;
            try
            {
                currentElection = GetElection();
                issues = GetIssues(currentElection);
                candidates = GetCandidates(currentElection);
                voter = GetVoter(username);
                status = CheckStatus(voter.Voter_Id);
            }
            catch
            {
                Console.WriteLine("An error occurred");
            }

            status = (status || voter.Voter_Id == -1 || voter.Voter_Id == 0) ? true : false;

            return status
                ? new Ballot(voter, currentElection, issues, candidates, true, new int[1], new int[1])
                : new Ballot(voter, currentElection, issues, candidates, false, new int[issues.Length], new int[candidates.Length]);
        }


        [HttpGet]
        public bool StoreResults(string jsonBallot)
        {
            Ballot ballot = JsonConvert.DeserializeObject<Ballot>(jsonBallot);
            Candidate votedCandidate = getVotedCandidate(ballot.Candidate_Results, ballot.Current_Election.electionId);
            Issue[] votedIssues = GetVotedIssues(ballot.Issues_Results, ballot.Current_Election.electionId);
            //use database instead because that's way easier
            try
            {
                StoreCandidateVote(votedCandidate, ballot);
                StoreIssueVotes(votedIssues, ballot);
            }
            catch (Exception exception)
            {
                if (exception is MySqlException)
                {
                    throw exception;
                }
                return false;
            }

            return true;
        }

        [HttpGet]
        public bool RemoveBallot(int id)
        {
            BallotAccessor ballotAccessor = new BallotAccessor(_configuration);
            try
            {
                ballotAccessor.RemoveBallot(id);
            }
            catch (Exception exception)
            {
                if (exception is MySqlException)
                {
                    throw exception;
                }

                return false;
            }
            return true;
        }

        [HttpGet]
        public string StoreResultsJSON(string jsonBallot)
        {
            Ballot ballot = JsonConvert.DeserializeObject<Ballot>(jsonBallot);
            Candidate votedCandidate = getVotedCandidate(ballot.Candidate_Results, ballot.Current_Election.electionId);
            Issue[] votedIssues = GetVotedIssues(ballot.Issues_Results, ballot.Current_Election.electionId);
            Candidate[] candidate = {votedCandidate};
            Ballot newBallot = new Ballot(ballot.Voter, ballot.Current_Election,votedIssues, candidate, false,
                new int[votedIssues.Length], new int[1]);
            return JsonConvert.SerializeObject(newBallot);
        }

        [HttpGet]
        public bool StoreVoted(int voterId, int electionId)
        {
            BallotAccessor ballotAccessor = new BallotAccessor(_configuration);
            try
            {
                ballotAccessor.StoreUserVoted(voterId, electionId);
            }
            catch (Exception exception)
            {
                if (exception is MySqlException)
                {
                    throw exception;
                }

                return false;
            }
            return true;
        }

        [HttpGet]
        public Ballot GeneratePastBallot(int voterId, int electionId)
        {
            BallotAccessor ballotAccessor = new BallotAccessor(_configuration);
            return ballotAccessor.GeneratePastBallot(voterId, electionId);
        }

        private void StoreIssueVotes(Issue[] issues, Ballot ballot)
        {
            BallotAccessor ballotAccessor = new BallotAccessor(_configuration);
            foreach (var issue in issues)
            {
                ballotAccessor.StoreResult(issue.Issue_Id, ballot.Voter.Voter_Id, ballot.Current_Election.electionId);
            }
        }

        private void StoreCandidateVote(Candidate candidate, Ballot ballot)
        {
            BallotAccessor ballotAccessor = new BallotAccessor(_configuration);
            ballotAccessor.StoreCandidate(candidate.canidate_Id, ballot.Voter.Voter_Id, ballot.Current_Election.electionId);
        }

        private Candidate getVotedCandidate(int[] results, int election)
        {
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] != -1)
                {
                    return GetCandidate(results[i], election);
                }
            }

            return new Candidate(-1, "", "", "", -1);
        }

        private Issue[] GetVotedIssues(int[] results, int election)
        {
            IssueAccessor issueAccessor = new IssueAccessor(_configuration);
            List<Issue> issues = new List<Issue>();
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] != -1)
                {
                    issues.Add(issueAccessor.GetIssue(results[i], election));
                }
            }

            return issues.ToArray();
        }

        private Election GetElection()
        {
            ElectionAccessor electionAccessor = new ElectionAccessor(_configuration);
            return electionAccessor.GetActiveElection();
        }

        private Issue[] GetIssues(Election currentElection)
        {
            IssueAccessor issueAccessor = new IssueAccessor(_configuration);
            return issueAccessor.GetIssues(currentElection.electionId);
        }

        private Candidate[] GetCandidates(Election currentElection)
        {
            CandidatesAccessor candidatesAccessor = new CandidatesAccessor(_configuration);
            return candidatesAccessor.GetCandidates(currentElection.electionId);
        }

        private Voter GetVoter(string username)
        {
            VoterAccessor voterAccessor = new VoterAccessor(_configuration);
            return voterAccessor.getVoter(username);
        }

        private bool CheckStatus(int voterId)
        {
            BallotAccessor ballotAccessor = new BallotAccessor(_configuration);
            return ballotAccessor.GetVoterStatus(voterId);
        }

        private Candidate GetCandidate(int id, int election)
        {
            CandidatesAccessor candidatesAccessor = new CandidatesAccessor(_configuration);
            return candidatesAccessor.GetCandidate(id, election);
        }

    }
}
