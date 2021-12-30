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
    public class ElectionAccessorUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public ElectionAccessorUnitTests()
        {
        }

        [TestMethod]
        public void GetAllElections()
        {
            ElectionAccessor accessor = new ElectionAccessor(configuration);
            Election[] elections = accessor.GetAllElections();
            Assert.IsTrue(elections.Length > 0);

        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void badConnectionGetAllElections()
        {
            ElectionAccessor accessor = new ElectionAccessor(badConfiguration);
            accessor.GetAllElections();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "No parameters given to function was inappropriately allowed")]
        public void getElectionNoParameters()
        {
            ElectionAccessor accessor = new ElectionAccessor(configuration);
            accessor.GetElection();
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void badconnectionGetElection()
        {
            ElectionAccessor accessor = new ElectionAccessor(badConfiguration);
            accessor.GetElection(default, default);
        }

        [TestMethod]
        public void getElectionById()
        {
            int id = 1;
            int year = -1;
            ElectionAccessor accessor = new ElectionAccessor(configuration);
            Election election = new Election(1, 2020, true);
            Election testElectionId = accessor.GetElection(id, year);
            Assert.IsTrue(testElectionId.electionId == election.electionId, "By ID returned: " +  testElectionId.electionId);
        }

        [TestMethod]
        public void GetElectionByYear()
        {
            int id = -1;
            int year = 2020;
            ElectionAccessor accessor = new ElectionAccessor(configuration);
            Election election = new Election(1, 2020, true);
            Election testElectionYear = accessor.GetElection(id, year);
            Assert.IsTrue(testElectionYear.electionId == election.electionId, "By year returned: " + testElectionYear.electionId);
        }

        [TestMethod]
        public void getBadElection()
        {
            ElectionAccessor accessor = new ElectionAccessor(configuration);
            Election election = accessor.GetElection(default, default);
            Assert.IsTrue(election.electionId == 0, "Returned an election with bad data given");
        }

        [TestMethod]
        public void GetActiveElection()
        {
            ElectionAccessor accessor = new ElectionAccessor(configuration);
            Election election = accessor.GetActiveElection();
            Assert.IsFalse(election.electionId == default);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetActiveElection()
        {
            ElectionAccessor accessor = new ElectionAccessor(badConfiguration);
            accessor.GetActiveElection();
        }
    }

}
