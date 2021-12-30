using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Accessors
{
    [TestClass]
    public class BallotAccessorUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public BallotAccessorUnitTests()
        {
        }

        [TestMethod]
        public void GetVoterStatus()
        {
            BallotAccessor accesor = new BallotAccessor(configuration);
            bool status = accesor.GetVoterStatus(1);
            Assert.IsTrue(status);
        }

        [TestMethod]
        public void NotVotedGetVoterStatus()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            bool status = accessor.GetVoterStatus(10000);
            Assert.IsFalse(status);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetVoterStatus()
        {
            BallotAccessor accessor = new BallotAccessor(badConfiguration);
            accessor.GetVoterStatus(default);
        }

        [TestMethod]
        public void StoreCandidate()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            try
            {
                accessor.StoreCandidate(1, 6, 1);
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception but got: " + exception.Message);
            }
            Cleanup(0);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Bad data was given that should not have modified the database")]

        public void BadDataStoreCandidate()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            accessor.StoreCandidate(default,default,default);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionStoreCandidate()
        {
            BallotAccessor accessor = new BallotAccessor(badConfiguration);
            accessor.StoreCandidate(default,default,default);

        }

        [TestMethod]
        public void StoreResult()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            try
            {
                accessor.StoreResult(1,6,1);
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception but got " + exception.Message);
            }
            Cleanup(0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Bad data was given that should not have modified the database")]
        public void BadDataStoreResult()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            accessor.StoreResult(default,default,default);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionStoreResult()
        {
            BallotAccessor accessor = new BallotAccessor(badConfiguration);
            accessor.StoreResult(default, default, default);
        }

        [TestMethod]
        public void RemoveBallot()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            accessor.StoreResult(1,6,1);
            try
            {
                accessor.RemoveBallot(6);
            }
            catch(Exception exception)
            {
                Assert.Fail("Expected no exception but got " + exception.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Bad data was given that should not have modified the database")]
        public void BadDataRemoveBallot()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            accessor.RemoveBallot(default);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionRemoveBallot()
        {
            BallotAccessor accessor = new BallotAccessor(badConfiguration);
            accessor.RemoveBallot(default);
        }

        [TestMethod]
        public void StoreUserVoted()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            try
            {
                accessor.StoreUserVoted(6,1);
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception but got " + e.Message);
            }
            Cleanup(1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Bad data was given that should not have modified the database")]
        public void BadDataStoreUserVoted()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            accessor.StoreUserVoted(default,default);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionStoreUserVoted()
        {
            BallotAccessor accessor = new BallotAccessor(badConfiguration);
            accessor.StoreUserVoted(default,default);
        }

        [TestMethod]
        public void GenerateBallot()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            Ballot ballot = accessor.GeneratePastBallot(1, 1);
            Assert.IsTrue(ballot.Candidates.Length > 0);
        }
        [TestMethod]
        public void BadDataGenerateBallot()
        {
            BallotAccessor accessor = new BallotAccessor(configuration);
            Ballot ballot = accessor.GeneratePastBallot(default, default);
            Assert.IsTrue(ballot.Candidates == null);
        }
        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGenerateBallot()
        {
            BallotAccessor accessor = new BallotAccessor(badConfiguration);
            accessor.GeneratePastBallot(default, default);
        }

        private void Cleanup(int mode)
        {
            MySqlConnection db = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            db.Open();
            MySqlCommand cmd = (mode == 0)? new MySqlCommand("DELETE FROM results WHERE results.voter_id=6;", db): new MySqlCommand("DELETE FROM voted_elections WHERE voted_elections.voter_id=6", db); ;
            int affected = cmd.ExecuteNonQuery();
            cmd.Dispose();
            if (affected == 0)
            {
                db.Close();
                throw new Exception("Could not delete data");
            }
            db.Close();

        }
    }
}
