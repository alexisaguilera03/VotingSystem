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
    public class IssueAccessorUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public IssueAccessorUnitTests()
        {
        }

        [TestMethod]
        public void GetIssues()
        {
            int electionId = 1;
            IssueAccessor accessor = new IssueAccessor(configuration);
            Issue[] issues = accessor.GetIssues(electionId);
            Assert.IsTrue(issues.Length > 0);
        }

        [TestMethod]
        public void BadGetIssues()
        {
            IssueAccessor accessor = new IssueAccessor(configuration);
            Issue[] issues = accessor.GetIssues(default);
            Assert.IsTrue(issues.Length == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetIssues()
        {
            IssueAccessor accessor = new IssueAccessor(badConfiguration);
            Issue[] issues = accessor.GetIssues(default);
        }

        [TestMethod]
        public void GetIssue()
        {
            IssueAccessor accessor = new IssueAccessor(configuration);
            Issue issue = accessor.GetIssue(1, 1);
            Assert.IsFalse(issue.Issue_Id == default);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGetIssue()
        {
            IssueAccessor accessor = new IssueAccessor(badConfiguration);
            accessor.GetIssue(default, default);
        }
    }
}
