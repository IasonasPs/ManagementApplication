namespace ManagementApplication.Models
{
    public class CandidateDegree
    {
        public Guid CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public Guid DegreeId { get; set; }
        public Degree Degree { get; set; }
    }
}
