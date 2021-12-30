using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Voting_System.Controllers;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Controllers
{
    [TestClass]
    public class BallotControllerUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public BallotControllerUnitTests()
        {
        }

        [TestMethod]
        public void GetBallot()
        {
            BallotController controller = new BallotController(configuration);
            Ballot ballot = controller.GetBallot("test", "test");
            Assert.IsTrue(!ballot.Voter_Status);
        }

        [TestMethod]
        public void BadLoginGetBallot()
        {
            BallotController controller = new BallotController(configuration);
            Ballot ballot = controller.GetBallot("", "");
            Assert.IsTrue(ballot.Voter_Status);
        }

        [TestMethod]
        public void BadConnectionGetBallot()
        {
            BallotController controller = new BallotController(badConfiguration);
            Ballot ballot = controller.GetBallot(default, default);
            Assert.IsTrue(ballot.Voter_Status);
        }

        [TestMethod]
        public void StoreResults()
        {
            Ballot ballot = GenerateBallot();
            BallotController controller = new BallotController(configuration);
            bool status = false;
            try
            {
                status = controller.StoreResults(JsonConvert.SerializeObject(ballot));
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception but got: " + e.Message);
            }
            
            Assert.IsTrue(status);
            Cleanup(0);
        }

        [TestMethod]
        public void BadDataStoreResults()
        {
            bool status = false;
            Ballot ballot = GenerateBallot();
            ballot.Voter = new Voter();
            BallotController controller = new BallotController(configuration);
            status = controller.StoreResults(JsonConvert.SerializeObject(ballot));
            Assert.IsTrue(!status);
        }
        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionStoreResults()
        {
            Ballot ballot = GenerateBallot();
            BallotController controller = new BallotController(badConfiguration);
            controller.StoreResults(JsonConvert.SerializeObject(ballot));
        }

        [TestMethod]
        public void RemoveBallot()
        {
            Ballot ballot = GenerateBallot();
            BallotController controller = new BallotController(configuration);
            bool status = controller.StoreResults(JsonConvert.SerializeObject(ballot));
            if (!status)
            {
                Assert.Fail("Controller could not write to database");
            }

            status = controller.RemoveBallot(ballot.Voter.Voter_Id);
            if (!status)
            {
                Assert.Fail("Controller could not delete data stored");
            }
        }

        [TestMethod]
        public void BadDataRemoveBallot()
        {
            BallotController controller = new BallotController(configuration);
            bool status = controller.RemoveBallot(default);
            Assert.IsTrue(!status);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionRemoveBallot()
        {
            BallotController controller = new BallotController(badConfiguration);
            controller.RemoveBallot(default);
        }

        [TestMethod]
        public void StoreResultsJSON()
        {
            Ballot ballot = GenerateBallot();
            BallotController controller = new BallotController(configuration);
            string json = controller.StoreResultsJSON(JsonConvert.SerializeObject(ballot));
            Ballot newBallot = JsonConvert.DeserializeObject<Ballot>(json);
            Assert.IsTrue(ballot.Voter.Voter_Id == newBallot.Voter.Voter_Id);
        }

        [TestMethod]
        public void StoreVoted()
        {
            BallotController controller = new BallotController(configuration);
            bool status = controller.StoreVoted(6, 1);
            Assert.IsTrue(status);
            Cleanup(1);
        }

        [TestMethod]
        public void BadDataStoreVoted()
        {
            BallotController controller = new BallotController(configuration);
            bool status = controller.StoreVoted(default, default);
            Assert.IsTrue(!status);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionStoreVoted()
        {
            BallotController controller = new BallotController(badConfiguration);
            controller.StoreVoted(default, default);
        }

        [TestMethod]
        public void GenerateBallotFrontEnd()
        {
            BallotController controller = new BallotController(configuration);
            Ballot ballot = controller.GeneratePastBallot(1, 1);
            Assert.IsTrue(ballot.Candidates != null);
        }

        [TestMethod]
        public void BadDataGenerateBallotFrontEnd()
        {
            BallotController controller = new BallotController(configuration);
            Ballot ballot = controller.GeneratePastBallot(default, default);
            Assert.IsTrue(ballot.Candidates == null);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGenerateBallotFrontEnd()
        {
            BallotController controller = new BallotController(badConfiguration);
            controller.GeneratePastBallot(default, default);

        }

        private void Cleanup(int mode)
        {
            MySqlConnection db = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            db.Open();
            MySqlCommand cmd = (mode == 0) ? new MySqlCommand("DELETE FROM results WHERE results.voter_id=6;", db) : new MySqlCommand("DELETE FROM voted_elections WHERE voted_elections.voter_id=6", db); ;
            int affected = cmd.ExecuteNonQuery();
            cmd.Dispose();
            if (affected == 0)
            {
                db.Close();
                throw new Exception("Could not delete data");
            }
            db.Close();

        }
        private Ballot GenerateBallot()
        {
            List<Issue> issues = new List<Issue>();
            issues.Add(new Issue(1,default,default,default,1));
            List<Candidate> candidates = new List<Candidate>();
            candidates.Add(new Candidate(1,default,default,default,1));
            Ballot ballot = new Ballot();
            ballot.Voter = new Voter(6, default, default, default, default, default);
            ballot.Issues = issues.ToArray();
            ballot.Candidates = candidates.ToArray();
            ballot.Current_Election = new Election(1, default, true);
            ballot.Voter_Status = false;
            ballot.Candidate_Results = new int[1];
            ballot.Issues_Results = new int[1];
            return ballot;
        }
    }
}
