using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Voting_System.Controllers;
using Voting_System.Models;

namespace Voting_System_Unit_Tests.UnitTests.Controllers
{
    [TestClass]
    public class LoginControllerUnitTests
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private IConfiguration badConfiguration = new ConfigurationBuilder().Build();

        public LoginControllerUnitTests()
        {

        }

        [TestMethod]
        public void Login()
        {
            string username = "alexisaguilera03";
            string password = "alexis1";
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login(username, password);
            Assert.IsTrue(user.Username == username);
            Assert.IsTrue(user.Password == password);
        }

        [TestMethod]
        public void BadUsernameLogin()
        {
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login("badusername", "alexis1");
            Assert.IsTrue(user.Username == "incorrect");
        }

        [TestMethod]
        public void BadPasswordLogin()
        {
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login("alexisaguilera03", "badpassword");
            Assert.IsTrue(user.Password == "incorrect");
        }

        [TestMethod]
        public void NoParametersLogin()
        {
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login();
            Assert.IsTrue(user.Username=="null" && user.Password == "null");
        }

        [TestMethod]
        public void UsernameWhiteSpaceLogin()
        {
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login("   ", "alexis1");
            Assert.IsTrue(user.Username == "missing");
        }

        [TestMethod]
        public void PasswordWhiteSpaceLogin()
        {
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login("alexisaguilera03", "      ");
            Assert.IsTrue(user.Password == "missing");
        }

        [TestMethod]
        public void UsernameUndefinedLogin()
        {
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login("undefined", "alexis1");
            Assert.IsTrue(user.Username == "missing");
        }

        [TestMethod]
        public void PasswordUndefinedLogin()
        {
            LoginController controller = new LoginController(configuration);
            Voter user = controller.Login("alexisaguilera03", "undefined");
            Assert.IsTrue(user.Password == "missing");
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "The database was opened when a bad connection was given")]
        public void BadConnectionLogin()
        {
            LoginController controller = new LoginController(badConfiguration);
            controller.Login("alexisaguilera03", "alexis1");
        }
    }
}
