using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Voting_System.Models;

namespace Voting_System.Accessors
{
    public class BallotAccessor: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BallotAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool GetVoterStatus(int id)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT * FROM voted_elections ve JOIN elections e ON ve.election_id = e.election_id " +
                         "WHERE active = 1 AND voter_id=@id;";
            MySqlConnection db = new MySqlConnection(conn);

            try
            {
                db.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("could not open database");
                throw exception;
            }

            MySqlCommand cmd = new MySqlCommand(sql, db);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                db.Close();
                return true;
            }
            else
            {
                db.Close();
                return false;
            }
        }

        public void StoreCandidate(int candidateId, int voterId, int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "INSERT INTO results " +
                         "(voter_id,candidate_id,election_id,candidate_result) " +
                         "VALUES(@voterId,@candidateId,@electionId,@result)";
            MySqlConnection db = new MySqlConnection(conn);
            int affected = 0;
            try
            {
                db.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("Could not open database");
                throw exception;
            }

            MySqlCommand cmd = new MySqlCommand(sql, db);
            cmd.Parameters.AddWithValue("@candidateId", candidateId);
            cmd.Parameters.AddWithValue("@voterId", voterId);
            cmd.Parameters.AddWithValue("electionId", electionId);
            cmd.Parameters.AddWithValue("@result", candidateId);
            try
            {
                 affected = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Could not store data to database");
            }
            
            cmd.Dispose();
            db.Close();
            if (affected == 0)
            {
                throw new Exception("Could not store data");
            }

        }

        public void StoreResult(int issueId, int voterId, int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "INSERT INTO results " +
                         "(voter_id,election_id,issue_id,issue_result) " +
                         "VALUES(@voterId,@electionId,@issueId,@result)";
            MySqlConnection db = new MySqlConnection(conn);
            int affected = 0;
            try
            {
                db.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("Could not open database");
                throw exception;
            }

            MySqlCommand cmd = new MySqlCommand(sql, db);
            cmd.Parameters.AddWithValue("@issueId", issueId);
            cmd.Parameters.AddWithValue("@voterId", voterId);
            cmd.Parameters.AddWithValue("@electionId", electionId);
            cmd.Parameters.AddWithValue("@result", issueId);
            try
            {
                affected = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Could not store data in database");
            } 
            
            cmd.Dispose();
            db.Close();
            if (affected == 0)
            {
                throw new Exception("Could not store data");
            }

        }

        public void RemoveBallot(int id)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "DELETE FROM results " +
                         "WHERE results.voter_id=@id";
            int affected = 0;
            MySqlConnection db = new MySqlConnection(conn);
            try
            {
                db.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("Could not open database");
                throw exception;
            }

            MySqlCommand cmd = new MySqlCommand(sql, db);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                affected = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("could not delete data in database");
            }
            
            cmd.Dispose();
            db.Close();
            if (affected == 0)
            {
                throw new Exception("Could not delete data");
            }
        }
        
        public void StoreUserVoted(int voterId, int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "INSERT INTO voted_elections " +
                         "(voter_id,election_id) " +
                         "VALUES(@voterId,@electionId)";
            MySqlConnection db = new MySqlConnection(conn);
            int affected = 0;
            try
            {
                db.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("Could not open database");
                throw exception;
            }

            MySqlCommand cmd = new MySqlCommand(sql, db);
            cmd.Parameters.AddWithValue("@voterId", voterId);
            cmd.Parameters.AddWithValue("@electionId", electionId);
            try
            {
                affected = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Could not store data in database");
            }
            
            cmd.Dispose();
            db.Close();
            if (affected == 0)
            {
                throw new Exception("Could not store data");
            }
        }
        [HttpGet]
        public Ballot GeneratePastBallot(int voterId, int electionId)
        {
            Ballot ballot = new Ballot();
            CandidatesAccessor candidatesAccessor = new CandidatesAccessor(_configuration);
            IssueAccessor issueAccessor = new IssueAccessor(_configuration);
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sqlCandidate = "SELECT results.voter_id, results.candidate_result " +
                                  "FROM voting_system.results " +
                                  "WHERE results.voter_id=@voterId " +
                                  "AND results.election_id=@electionId " +
                                  "AND (NOT(results.candidate_result)=0);";
            string sqlIssues = "SELECT results.voter_id, results.issue_result " +
                               "FROM voting_system.results " +
                               "WHERE results.voter_id=@voterId " +
                               "AND results.election_id=@electionId " +
                               "AND (NOT (results.issue_result)=0);";

            MySqlConnection db = new MySqlConnection(conn);
            try
            {
                db.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("Could not open database");
                throw exception;
            }

            MySqlCommand cmdCandidate = new MySqlCommand(sqlCandidate, db);
            MySqlCommand cmdIssue = new MySqlCommand(sqlIssues, db);
            cmdCandidate.Parameters.AddWithValue("@voterId", voterId);
            cmdCandidate.Parameters.AddWithValue("@electionId", electionId);
            cmdIssue.Parameters.AddWithValue("@voterId", voterId);
            cmdIssue.Parameters.AddWithValue("@electionId", electionId);
            MySqlDataReader reader = cmdCandidate.ExecuteReader();

            while (reader.Read())
            {
                //Voter Voter = new Voter(); //this data is not needed and is thus left blank
                //Election Current_Election = new Election(); //^^^
                Candidate[] Candidates = {candidatesAccessor.GetCandidate(reader.GetInt32(1), electionId) };
                ballot.Candidates = Candidates;
            }
            reader.Close();

            reader = cmdIssue.ExecuteReader();
            List < Issue > issues = new List<Issue>();
            while (reader.Read())
            {
                issues.Add(issueAccessor.GetIssue(reader.GetInt32(1), electionId));

            }

            ballot.Issues = issues.ToArray();
            return ballot;
        }



    }
}
