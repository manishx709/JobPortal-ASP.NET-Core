using Microsoft.AspNetCore.Http;

namespace JobPortal.API.DTOs
{
    public class CreateJobDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string CompanyName { get; set; }

        public string Location { get; set; }

        public decimal Salary { get; set; }

        public IFormFile? Logo { get; set; }
    }
}