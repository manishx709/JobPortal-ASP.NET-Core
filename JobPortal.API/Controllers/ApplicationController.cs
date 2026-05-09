using JobPortal.API.DTOs;
using JobPortal.Models.Entities;
using JobPortal.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository
            _applicationRepository;

        public ApplicationController(
            IApplicationRepository applicationRepository)
        {
            _applicationRepository =
                applicationRepository;
        }
        [Authorize(Roles = "Candidate")]
        [HttpPost("apply/{jobId}")]
        public async Task<IActionResult> ApplyJob(
            int jobId,
            [FromForm] ApplyJobDTO model)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (model.Resume == null ||
                model.Resume.Length == 0)
            {
                return BadRequest("Resume is required.");
            }

            var fileName =
                Guid.NewGuid().ToString() +
                Path.GetExtension(model.Resume.FileName);

            var uploadPath =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads",
                    fileName);

            using (var stream =
                   new FileStream(uploadPath, FileMode.Create))
            {
                await model.Resume.CopyToAsync(stream);
            }

            Application application = new()
            {
                JobId = jobId,

                UserId = userId!,

                ResumePath = "/uploads/" + fileName,

                Status = "Pending",

                AppliedDate = DateTime.Now
            };

            var result =
                await _applicationRepository
                    .ApplyJobAsync(application);

            return Ok(result);
        }
        [Authorize(Roles = "Candidate")]
        
        [HttpGet("my-applications")]
        public async Task<IActionResult> MyApplications()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var applications =
                await _applicationRepository
                    .GetApplicationsByUserAsync(userId);

            var result =
                applications.Select(a => new
                {
                    a.Id,

                    a.JobId,

                    JobTitle = a.Job.Title,

                    CompanyName = a.Job.CompanyName,

                    a.AppliedDate,

                    a.ResumePath,

                    a.Status
                });

            return Ok(result);
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult>
            GetApplicationsByJob(int jobId)
        {
            var applications =
                await _applicationRepository
                    .GetApplicationsByJobAsync(jobId);

            return Ok(applications);
        }
        [Authorize(Roles = "Employer")]
        [HttpPut("accept/{id}")]
        public async Task<IActionResult>
    AcceptApplication(int id)
        {
            var application =
                await _applicationRepository
                    .UpdateStatusAsync(id, "Accepted");

            if (application == null)
            {
                return NotFound("Application not found.");
            }

            return Ok(application);
        }
        [Authorize(Roles = "Employer")]
        [HttpPut("reject/{id}")]
        public async Task<IActionResult>
    RejectApplication(int id)
        {
            var application =
                await _applicationRepository
                    .UpdateStatusAsync(id, "Rejected");

            if (application == null)
            {
                return NotFound("Application not found.");
            }

            return Ok(application);
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("employer-applications")]
        public async Task<IActionResult>
    EmployerApplications()
        {
            var employerId =
      User.FindFirstValue(
          ClaimTypes.NameIdentifier);

            var applications =
                await _applicationRepository
                    .GetApplicationsByEmployerAsync(
                        employerId);
            var result =
                applications.Select(a => new
                {
                    a.Id,

                    CandidateName = a.User.FullName,

                    CandidateEmail = a.User.Email,

                    JobTitle = a.Job.Title,

                    a.AppliedDate,

                    a.ResumePath,

                    a.Status
                });

            return Ok(result);
        }
    }
}