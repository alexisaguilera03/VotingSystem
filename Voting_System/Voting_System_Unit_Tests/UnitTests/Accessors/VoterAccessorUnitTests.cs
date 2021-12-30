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
    public class VoterAccessorUnitTests
    {
          IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();


        public VoterAccessorUnitTests()
        {
        }

        [TestMethod]
        public void GetVoters()
        {
            VoterAccessor accessor = new VoterAccessor(configuration);
            int mockElectionId = 1;
            Voter[] mockVoters = accessor.GetVoters(mockElectionId);
            Assert.IsTrue(mockVoters.Length > 0);
        }

        [TestMethod]
        public void GetBadVoters()
        {
            int electionId = -1;
            VoterAccessor accessor = new VoterAccessor(configuration);
            Voter[] voters = accessor.GetVoters(electionId);
            Assert.IsTrue(voters.Length == 0);
        }

        [TestMethod]
        public void getVoter()
        {
            string username = "alexisaguilera03";
            Voter voter = new Voter(1, "Alexis", "AguileraOrtiz", "1234 street Lincoln Ne 68521", "alexisaguilera03", "alexis1");
            VoterAccessor accessor = new VoterAccessor(configuration);
            Voter testVoter = accessor.getVoter(username);
            Assert.IsTrue(voter.Voter_Id == testVoter.Voter_Id);
            Assert.IsTrue(voter.First_Name == testVoter.First_Name);
            Assert.IsTrue(voter.Last_Name == testVoter.Last_Name);
            Assert.IsTrue(voter.Address == testVoter.Address);


        }

        [TestMethod]
        public void getBadVoter()
        {
            VoterAccessor accessor = new VoterAccessor(configuration);
            Voter testBadVoter = accessor.getVoter("badusername");
            Assert.IsTrue(testBadVoter.Voter_Id == -1);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void failedConnectionGetVoter()
        {
            IConfiguration mockConfiguration = new ConfigurationBuilder().Build();
            VoterAccessor accessor = new VoterAccessor(mockConfiguration);
            accessor.getVoter("test");
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void failedConnectionGetVoters()
        {
            IConfiguration mockConfiguration = new ConfigurationBuilder().Build();
            VoterAccessor accessor = new VoterAccessor(mockConfiguration);
            accessor.GetVoters(-1);
        }
    }
}
