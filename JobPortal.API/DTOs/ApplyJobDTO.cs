using Microsoft.AspNetCore.Http;

namespace JobPortal.API.DTOs
{
    public class ApplyJobDTO
    {
        public IFormFile Resume { get; set; }
    }
}