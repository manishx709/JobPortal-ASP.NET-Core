using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models.Entities
{
    public class Application
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public Job? Job { get; set; }

        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public string ResumePath { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime AppliedDate { get; set; }
    }
}