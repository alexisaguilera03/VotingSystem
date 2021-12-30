using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Voting_System.Models;

namespace Voting_System.Accessors
{
    public class ElectionAccessor: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ElectionAccessor(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public Election GetElection(int id = -1, int year = -1)
        {
            if (id == -1 && year == -1)
            {
                throw new ArgumentException("No parameters given");
            }
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql;
            if (id != -1 && year != -1)
            {
                sql = "SELECT * FROM elections WHERE election_id=@ID;";
            }
            else
            {
                sql = (id != -1) ? "SELECT * FROM elections WHERE election_id=@id;" : "SELECT * FROM elections WHERE election_year=@year;";
            }

            using MySqlConnection db = new MySqlConnection(conn);
            Election currentElection = new Election();
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
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@year", year);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int electionId = reader.GetInt32(0);
                int electionYear = reader.GetInt32(1);
                bool active = reader.GetBoolean(2);
                currentElection = new Election(electionId, electionYear, active);
            }
            db.Close();
            return currentElection;
        }

        public Election[] GetAllElections()
        {
            List<Election> elections = new List<Election>();
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT * FROM elections;";
            using MySqlConnection db = new MySqlConnection(conn);
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
            
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int electionId = reader.GetInt32(0);
                int electionYear = reader.GetInt32(1);
                bool active = reader.GetBoolean(2);
                elections.Add(new Election(electionId, electionYear, active));
            }

            return elections.ToArray();

        }
        public Election GetActiveElection()
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "SELECT * FROM elections " +
                         "WHERE elections.active=true;";
            Election currentElection = new Election();
            MySqlConnection db = new MySqlConnection(conn);
            MySqlCommand cmd = new MySqlCommand(sql, db);
            Election election = new Election();

            try
            {
                db.Open();

            }
            catch (MySqlException exception)
            {
                Console.WriteLine("could not open database");
                throw exception;
            }

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int electionId = reader.GetInt32(0);
                int electionYear = reader.GetInt32(1);
                bool active = reader.GetBoolean(2);
                election = new Election(electionId, electionYear, active);
            }

            return election;

        }
    }
}
