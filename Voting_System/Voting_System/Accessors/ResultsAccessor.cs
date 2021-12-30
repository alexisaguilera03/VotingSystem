using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Voting_System.Models;

namespace Voting_System.Accessors
{
    public class ResultsAccessor
    {
        private readonly IConfiguration _configuration;

        public ResultsAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void  getCandidateVotes(ref Candidate candidate)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT results.voter_id, results.candidate_result " +
                         "FROM results " +
                         "WHERE results.candidate_id=@id and (NOT(results.candidate_result)=0)";
            using (MySqlConnection db = new MySqlConnection(conn))
            {
                try
                {
                    db.Open();
                }
                catch(MySqlException exception)
                {
                    Console.WriteLine("Could not open database");
                    throw exception;
                }

                MySqlCommand cmd = new MySqlCommand(sql, db);
                cmd.Parameters.AddWithValue("@id", candidate.canidate_Id);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    candidate.Votes++;
                }
                db.Close();
                return;
            }
        }

        public void getIssueVotes(ref Issue issue)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT results.voter_id, results.issue_result " +
                         "FROM results " +
                         "WHERE results.issue_id=@id and (NOT(results.issue_result)=0);";
            using (MySqlConnection db = new MySqlConnection(conn))
            {
                try
                {
                    db.Open();
                }
                catch(MySqlException exception)
                {
                    Console.WriteLine("Could not open database");
                    throw exception;
                }

                MySqlCommand cmd = new MySqlCommand(sql, db);
                cmd.Parameters.AddWithValue("@id", issue.Issue_Id);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    issue.Votes++;
                }

                db.Close();
                return;
            }

        }


    }
}
