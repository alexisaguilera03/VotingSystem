using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Voting_System.Models;

namespace Voting_System.Accessors
{
    public class CandidatesAccessor
    {
        private readonly IConfiguration _configuration;

        public CandidatesAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Candidate[] GetCandidates(int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT candidates.candidate_id, candidates.first_name, candidates.last_name, candidates.affiliation " +
                         "FROM elections INNER JOIN candidates " +
                         "ON elections.election_id = candidates.assigned_election " +
                         "WHERE candidates.assigned_election=@id;";

            using (MySqlConnection db = new MySqlConnection(conn))
            {
                List<Candidate> candidates = new List<Candidate>();
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
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string fname = reader.GetString(1);
                    string lname = reader.GetString(2);
                    string affiliation = reader.GetString(3);
                    candidates.Add(new Candidate(id, fname, lname, affiliation, electionId));
                }
                db.Close();
                return candidates.ToArray();
            }
        }

        public Candidate GetCandidate(int id, int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT * " +
                         "FROM candidates " +
                         "WHERE candidates.candidate_id=@id";
            Candidate candidate = new Candidate();
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
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int candidateId = reader.GetInt32(0);
                string fname = reader.GetString(1);
                string lname = reader.GetString(2);
                string affiliation = reader.GetString(3);
                candidate = new Candidate(candidateId, fname, lname, affiliation, electionId);
            }

            return candidate;

        }
    }
}
