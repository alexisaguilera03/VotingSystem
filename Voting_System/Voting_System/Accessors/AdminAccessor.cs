using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Voting_System.Models;

namespace Voting_System.Accessors
{
    public class AdminAccessor : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int ChangeActiveElection(int electionId)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");

            //make all elections inactive first
            string sqlClear = "UPDATE elections SET elections.active = 0;";

            string sqlChange = "UPDATE elections e SET e.active = 1 " +
                               "WHERE e.election_id = @id;";
            using MySqlConnection db = new MySqlConnection(conn);

            try
            {
                db.Open();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("Could not open database");
                throw exception;
            }
            MySqlCommand cmdClear = new MySqlCommand(sqlClear, db);
            MySqlCommand cmdChange = new MySqlCommand(sqlChange, db);

            try
            {
                cmdChange.Parameters.AddWithValue("@id", electionId);
                cmdClear.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error executing query");
                throw e;
            }

            int rows_affected = cmdChange.ExecuteNonQuery();
            return rows_affected;
        }
        public int AddCandidate(string first_name, string last_name, string affiliation, int election_id)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "INSERT INTO candidates (first_name, last_name, affiliation, assigned_election) " +
                         "VALUES (@first, @last, @affiliation, @id);";
            string sqlKey = "SELECT LAST_INSERT_ID();";
            using MySqlConnection db = new MySqlConnection(conn);

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
            MySqlCommand keyCmd = new MySqlCommand(sqlKey, db);

            cmd.Parameters.AddWithValue("@first", first_name);
            cmd.Parameters.AddWithValue("@last", last_name);
            cmd.Parameters.AddWithValue("@affiliation", affiliation);
            cmd.Parameters.AddWithValue("@id", election_id);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error executing query");
                throw e;
            }

            var reader = keyCmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            return 0;
        }
        public int AddIssue(string issue_description, string issue_title, int election_id)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "INSERT INTO issues (issue_description, issue_title, election_Id) " +
                         "VALUES (@desc, @title, @id);";
            string sqlKey = "SELECT LAST_INSERT_ID();";
            using MySqlConnection db = new MySqlConnection(conn);

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
            MySqlCommand keyCmd = new MySqlCommand(sqlKey, db);

            cmd.Parameters.AddWithValue("@desc", issue_description);
            cmd.Parameters.AddWithValue("@title", issue_title);
            cmd.Parameters.AddWithValue("@id", election_id);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error executing query");
                throw e;
            }
            var reader = keyCmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            return 0;
        }
        public int AddElection(int year)
        {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            string sql = "INSERT INTO elections (election_year) " +
                         "VALUES (@year);";
            string sqlKey = "SELECT LAST_INSERT_ID();";
            using MySqlConnection db = new MySqlConnection(conn);

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
            MySqlCommand keyCmd = new MySqlCommand(sqlKey, db);

            cmd.Parameters.AddWithValue("@year", year);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error executing query");
                throw e;
            }
            var reader = keyCmd.ExecuteReader();

            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            return 0;
        }
    }
}
