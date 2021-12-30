
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Accessors
{
    [TestClass]
    public class CandidateAccessorUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public CandidateAccessorUnitTests()
        {
        }

        [TestMethod]
        public void GetCandidates()
        {
            int electionId = 1;
            CandidatesAccessor accessor = new CandidatesAccessor(configuration);
            Candidate[] candidates = accessor.GetCandidates(electionId);
            Assert.IsTrue(candidates.Length > 0);
        }

        [TestMethod]
        public void GetBadCandidates()
        {
            int electionId = -1;
            CandidatesAccessor accessor = new CandidatesAccessor(configuration);
            Candidate[] candidates = accessor.GetCandidates(electionId);
            Assert.IsTrue(candidates.Length < 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetCandidates()
        {
            CandidatesAccessor accessor = new CandidatesAccessor(badConfiguration);
            Candidate[] candidates = accessor.GetCandidates(default);
        }

        [TestMethod]
        public void GetCandidate()
        {
            CandidatesAccessor accessor = new CandidatesAccessor(configuration);
            Candidate candidate = accessor.GetCandidate(1, 1);
            Assert.IsFalse(candidate.canidate_Id == 0);
        }

        [TestMethod]
        public void GetBadCandidate()
        {
            CandidatesAccessor accessor = new CandidatesAccessor(configuration);
            Candidate candidate = accessor.GetCandidate(-1, -1);
            Assert.IsTrue(candidate.canidate_Id == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetCandidate()
        {
            CandidatesAccessor accessor = new CandidatesAccessor(badConfiguration);
            accessor.GetCandidate(default, default);
        }
    }
}
