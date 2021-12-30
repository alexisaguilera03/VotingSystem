using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Voting_System.Models;

namespace Voting_System.Accessors
{
    public class IssueAccessor
    {
        private readonly IConfiguration _configuration;

        public IssueAccessor(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public Issue[] GetIssues(int electionId)
        {

            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT issues.issue_id, issues.issue_description, issues.issue_title " +
                         "FROM elections INNER JOIN issues " +
                         "ON elections.election_id = issues.election_id " +
                         "WHERE issues.election_id=@id;";
            using (MySqlConnection db = new MySqlConnection(conn))
            {
                List<Issue> issues = new List<Issue>();
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
                cmd.Parameters.AddWithValue("@id", electionId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string description = reader.GetString(1);
                    string title = reader.GetString(2);
                    issues.Add(new Issue(id, description, title, electionId));
                }
                db.Close();
                return issues.ToArray();
            }
        }

        public Issue GetIssue(int issueId, int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT * " +
                         "FROM issues " +
                         "WHERE issues.issue_id=@id";
            Issue issue = new Issue(-1, "", "", -1);
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
            cmd.Parameters.AddWithValue("@id", issueId);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string description = reader.GetString(1);
                string title = reader.GetString(2);
                issue = new Issue(id, description, title, electionId);
            }

            return issue;
        }
    }
}
