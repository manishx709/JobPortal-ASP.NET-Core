using JobPortal.API.DTOs;
using JobPortal.Data;
using JobPortal.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobPortal.Models.Entities;


namespace JobPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _jobRepository.GetAllJobsAsync();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(int id)
        {

            var job = await _jobRepository.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateJob(
    [FromForm] CreateJobDTO model)
        {
            var employerId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            string? logoPath = null;

            if (model.Logo != null)
            {
                var fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(
                        model.Logo.FileName);

                var folderPath =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/logos");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(
                        folderPath);
                }

                var filePath =
                    Path.Combine(
                        folderPath,
                        fileName);

                using (var stream =
                       new FileStream(
                           filePath,
                           FileMode.Create))
                {
                    await model.Logo.CopyToAsync(
                        stream);
                }

                logoPath =
                    "/logos/" + fileName;
            }

            Job job = new()
            {
                Title = model.Title,

                Description = model.Description,

                Salary = model.Salary,

                Location = model.Location,

                CompanyName = model.CompanyName,

                PostedDate = DateTime.Now,

                EmployerId = employerId!,

                CompanyLogo = logoPath
            };

            var createdJob =
                await _jobRepository
                    .CreateJobAsync(job);

            return Ok(createdJob);
        }

        [Authorize(Roles = "Employer")]
        [HttpDelete("{id}")]
        public IActionResult DeleteJob(int id)
        {
            return Ok($"Job with ID {id} deleted successfully.");
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost("apply/{jobId}")]
        public IActionResult ApplyJob(int jobId)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new{
                Message = "Application submitted successfully",
                CandidateID = userId,
                JobId = jobId}
                );
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("employer-dashboard")]
        public IActionResult EmployerDashboard()
        {
            return Ok("Welcome Employer Dashboard");
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("candidate-dashboard")]
        public IActionResult CandidateDashboard()
        {
            return Ok("Welcome Candidate Dashboard");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminPanel()
        {
            return Ok("Welcome Admin");
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchJobs(
    string? title,
    string? location,
    decimal? minSalary)
        {
            var jobs =
                await _jobRepository.SearchJobsAsync(
                    title,
                    location,
                    minSalary);

            return Ok(jobs);
        }
        [HttpGet("paged")]
        public async Task<IActionResult>
    GetPagedJobs(
        int pageNumber = 1,
        int pageSize = 5)
        {
            var jobs =
                await _jobRepository
                    .GetPagedJobsAsync(
                        pageNumber,
                        pageSize);

            return Ok(jobs);
        }
    }
}