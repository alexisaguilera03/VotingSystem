
using System.ComponentModel.DataAnnotations;


namespace Voting_System.Models
{
    public class Voter
    {
        [Key]
        public int Voter_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set;  }

        public Voter()
        {

        }

        public Voter(int id, string first, string last, string address, string username, string password)
        {
            Voter_Id = id;
            First_Name = first;
            Last_Name = last;
            Address = address;
            Username = username;
            Password = password;

        }


    }
}
