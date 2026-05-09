using JobPortal.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class Job
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }
        public string Location { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
    public string? CompanyLogo { get; set; }
    public DateTime PostedDate { get; set; }= DateTime.Now;
        public string EmployerId { get; set; }= string.Empty;
    [ForeignKey("EmployerId")]
    public ApplicationUser? Employer { get; set; }//🔥 WHAT THIS CREATES

    //    Relationship/ApplicationUser(Employer)Jobs One employer can post MANY jobs.




}

