namespace JobPortal.Web.Models
{
    public class EmployerApplicationDTO
    {
        public int Id { get; set; }

        public string CandidateName { get; set; }

        public string CandidateEmail { get; set; }

        public string JobTitle { get; set; }

        public DateTime AppliedDate { get; set; }

        public string ResumePath { get; set; }

        public string Status { get; set; }
    }
}