using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Voting_System.Models;
using Microsoft.Extensions.Configuration;

namespace Voting_System.Accessors
{
    public class VoterAccessor : ControllerBase
    {
        private readonly IConfiguration _configuration;


        public VoterAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Voter[] GetVoters(int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql =
                "SELECT voters.voter_id, voters.first_name, voters.last_name, voters.address, voters.username, voters.password " +
                "FROM voters INNER JOIN voted_elections " +
                "ON voters.voter_id = voted_elections.voter_id " +
                "WHERE voted_elections.election_id=@id;";

            using (MySqlConnection db = new MySqlConnection(conn)) 
            {
                List <Voter> voters = new List<Voter>();
                try
                {
                    db.Open();
                }
                catch(MySqlException exception)
                {
                    Console.WriteLine("Could not access database");
                    throw exception;
                }

                MySqlCommand cmd = new MySqlCommand(sql, db);
                cmd.Parameters.AddWithValue("@id", electionId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string fname = reader.GetString(1);
                    string lname = reader.GetString(2);
                    string address = reader.GetString(3);
                    string username = reader.GetString(4);
                    string password = reader.GetString(5);
                    voters.Add(new Voter(id, fname, lname, address, username, password));
                }

                return voters.ToArray();
            }
        }

        public Voter getVoter(string username)
        {
            username = username.Replace(" ", ""); //I don't know why but the ballot component inserts an empty space before the username
            string conn = _configuration.GetConnectionString("DefaultConnection");
            MySqlConnection db = new MySqlConnection(conn);
            Voter voter = new Voter(-1, "null", "null", "null", "incorrect", "incorrect");
            string sql = "SELECT * " +
                         "FROM voters " +
                         "WHERE voters.username=@username";
            try
            {
                db.Open();
            }
            catch(MySqlException exception)
            {
                Console.WriteLine("Could not access database");
                throw exception; //create further error handling?
            }

            MySqlCommand cmd = new MySqlCommand(sql, db);
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string fname = reader.GetString(1);
                string lname = reader.GetString(2);
                string address = reader.GetString(3);
                string usrname = reader.GetString(4);
                string password = reader.GetString(5);
                voter = (username == usrname) ? new Voter(id, fname, lname, address, usrname, password) : voter;

            }
            db.Close();

            return voter;
        }
    }
}
