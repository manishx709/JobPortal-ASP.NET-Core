namespace JobPortal.Web.Models
{
    public class JobDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public string Location { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; }
        public string? CompanyLogo { get; set; }
    }
}