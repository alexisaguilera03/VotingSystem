using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Voting_System.Accessors;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Accessors
{
    [TestClass]
    public class ResultsAccessorUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public ResultsAccessorUnitTests()
        {
        }

        [TestMethod]
        public void GetCandidateVotes()
        {
            CandidatesAccessor candidatesAccessor = new CandidatesAccessor(configuration);
            ResultsAccessor resultsAccessor = new ResultsAccessor(configuration);
            Candidate candidate = candidatesAccessor.GetCandidates(1)[0];
            resultsAccessor.getCandidateVotes(ref candidate);
            Assert.IsTrue(candidate.Votes > 0);
        }

        [TestMethod]
        public void BadDataGetCandidateVotes()
        {
            ResultsAccessor accessor = new ResultsAccessor(configuration);
            Candidate candidate = new Candidate(-1, "", "", "", -1);
            accessor.getCandidateVotes(ref candidate);
            Assert.IsTrue(candidate.Votes == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetCandidateVotes()
        {
            ResultsAccessor accessor = new ResultsAccessor(badConfiguration);
            Candidate candidate = new Candidate();
            accessor.getCandidateVotes(ref candidate);
        }

        [TestMethod]
        public void GetIssueVotes()
        {
            IssueAccessor issueAccessor = new IssueAccessor(configuration);
            Issue issue = issueAccessor.GetIssues(1)[0];
            ResultsAccessor resultsAccessor = new ResultsAccessor(configuration);
            resultsAccessor.getIssueVotes(ref issue);
            Assert.IsTrue(issue.Votes > 0);
        }

        [TestMethod]
        public void BadDataGetIssueVotes()
        {
            Issue issue = new Issue(-1,"","",-1);
            ResultsAccessor accessor = new ResultsAccessor(configuration);
            accessor.getIssueVotes(ref issue);
            Assert.IsTrue(issue.Votes == 0, "Unexpected Votes");
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetIssueVotes()
        {
            ResultsAccessor accessor = new ResultsAccessor(badConfiguration);
            Issue issue = new Issue(-1, "", "", -1);
            accessor.getIssueVotes(ref issue);
        }
    }
}
