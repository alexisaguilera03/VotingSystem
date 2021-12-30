using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Voting_System.Accessors;
using Voting_System.Controllers;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Controllers
{
    [TestClass]
    public class ResultControllerUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public ResultControllerUnitTests()
        {
        }

        [TestMethod]
        public void Get()
        {
            ResultsController controller = new ResultsController(configuration);
            int id = 1;
            int year = 2020;
            Result result = controller.Get(id, year);
            Assert.IsTrue(result.Election.electionId == id);
            Assert.IsTrue(result.Passed_Issues.Length > 0);
            Assert.IsTrue(result.Winner.Assigned_Election == id);
        }


        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionGet()
        {
            ResultsController controller = new ResultsController(badConfiguration);
            controller.Get(default, default);
        }

    }


}
