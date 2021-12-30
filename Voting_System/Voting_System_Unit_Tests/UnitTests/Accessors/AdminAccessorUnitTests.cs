using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Accessors
{
    [TestClass]
    public class AdminAccessorUnitTests
    {
        private IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public AdminAccessorUnitTests()
        {
        }

        [TestMethod]
        public void ChangeActiveElection()
        {
            AdminAccessor accesor = new AdminAccessor(configuration);
            try
            {
                int lines_changed = accesor.ChangeActiveElection(2);
                Assert.AreEqual(lines_changed, 1);
            } catch(MySqlException e)
            {
                Assert.Fail("Expect no exception, but got: " + e.Message);
            }
            Reset();

        }

        [TestMethod]
        public void ChangeActiveToCurrentActive()
        {
            AdminAccessor accesor = new AdminAccessor(configuration);
            try
            {
                int lines_changed = accesor.ChangeActiveElection(1);
                Assert.AreEqual(lines_changed, 1);
            }
            catch (MySqlException e)
            {
                Assert.Fail("Expect no exception, but got: " + e.Message);
            }
            Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Could not open database")]
        public void BadChangeActiveElection()
        {
            AdminAccessor accessor = new AdminAccessor(badConfiguration);
            accessor.ChangeActiveElection(2);
            Reset();
        }

        [TestMethod]
        public void ChangeActiveInvalidElection()
        {
            AdminAccessor accessor = new AdminAccessor(configuration);
            int primary = accessor.ChangeActiveElection(10);
            Assert.AreEqual(primary, 0);
            Reset();
        }

        [TestMethod]
        public void AddValidCandidate()
        {
            AdminAccessor accesor = new AdminAccessor(configuration);
            int primary = accesor.AddCandidate("Shooty", "McShootFace", "Guns", 1);
            Assert.AreEqual(primary, 5);
            Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Error executing query")]
        public void AddInvalidCandidate()
        {
            AdminAccessor accesor = new AdminAccessor(configuration);
            int primary = accesor.AddCandidate("", "", "", 0);
            Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Error executing query")]
        public void BadAddCandidate()
        {
            AdminAccessor accesor = new AdminAccessor(badConfiguration);
            int primary = accesor.AddCandidate("", "", "", 1);
            Reset();
        }

        [TestMethod]
        public void AddValidElection()
            {
            AdminAccessor accesor = new AdminAccessor(configuration);
            int primary = accesor.AddElection(2000);
            Assert.AreEqual(primary, 3);
            Reset();
            }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Error opening database")]
        public void BadAddElection()
        {
            AdminAccessor accesor = new AdminAccessor(badConfiguration);
            int primary = accesor.AddElection(2000);
            Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Error executing query")]
        public void AddInvalidElection()
        {
            AdminAccessor accesor = new AdminAccessor(badConfiguration);
            int primary = accesor.AddElection(-50);
            Reset();
        }

        [TestMethod]
        public void AddValidIssue()
        {
            AdminAccessor accesor = new AdminAccessor(configuration);
            int primary = accesor.AddIssue("", "", 1);
            Assert.AreEqual(primary, 4);
            Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Error openning database")]
        public void BadAddIssue()
        {
            AdminAccessor accesor = new AdminAccessor(badConfiguration);
            int primary = accesor.AddIssue("", "", 1);
            Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Error executing query")]
        public void AddInvalidIssue()
        {
            AdminAccessor accesor = new AdminAccessor(badConfiguration);
            int primary = accesor.AddIssue("", "", 10);
            Reset();
        }


        public void Reset()
        {
            using MySqlConnection db = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            db.Open();
            MySqlCommand resetActive = new MySqlCommand("UPDATE elections SET elections.active = 0; " + 
                "UPDATE elections e SET e.active = 1 WHERE e.election_id = 1;", db);
            resetActive.ExecuteNonQuery();

            MySqlCommand resetCand = new MySqlCommand("DELETE FROM candidates WHERE candidates.candidate_id > 4;", db);
            resetCand.ExecuteNonQuery();

            MySqlCommand resetElection = new MySqlCommand("DELETE FROM elections WHERE elections.election_id > 2;", db);
            resetElection.ExecuteNonQuery();

            MySqlCommand resetIssue = new MySqlCommand("DELETE FROM issues WHERE issues.issue_id > 3;", db);
            resetIssue.ExecuteNonQuery();

            MySqlCommand resetIncrement = new MySqlCommand("ALTER TABLE candidates AUTO_INCREMENT = 5; " +
                "ALTER TABLE elections AUTO_INCREMENT = 3; " +
                "ALTER TABLE issues AUTO_INCREMENT = 4;", db);
            resetIncrement.ExecuteNonQuery();
        }
    }
}
