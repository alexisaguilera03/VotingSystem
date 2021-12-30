
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Voting_System.Controllers;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Controllers
{
    [TestClass]
    public class ElectionControllerUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public ElectionControllerUnitTests()
        {
        }

        [TestMethod]
        public void GetElections()
        { 
            ElectionController controller = new ElectionController(configuration);
            Election[] elections = controller.GetElections();
            Assert.IsTrue(elections.Length > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")] //rewriting this unit test may be necessary if we handle this error in the future
        public void BadConnectionGetElections()
        {
            ElectionController controller = new ElectionController(badConfiguration);
            Election[] elections = controller.GetElections();
        }
        
    }
}
