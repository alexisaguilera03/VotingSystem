
namespace Voting_System.Models
{
    public class Issue
    {
        public int Issue_Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int Election_Id { get; set; }
        public int Votes { get; set; }
        public bool Passed { get; set; }

        public Issue(int issueId, string description, string title, int electionId, int votes = 0, bool passed = false)
        {
            Issue_Id = issueId;
            Description = description;
            Title = title;
            Election_Id = electionId;
            Votes = votes;
            Passed = passed;
        }

   

        
    }

}
