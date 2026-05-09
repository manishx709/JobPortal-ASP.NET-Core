namespace JobPortal.Web.Models
{
    public class ApplicationDTO
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public string JobTitle { get; set; }

        public string CompanyName { get; set; }

        public DateTime AppliedDate { get; set; }

        public string ResumePath { get; set; }

        public string Status { get; set; }
    }
}