using JobPortal.Data;
using JobPortal.Models.Entities;
using JobPortal.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repository.Repository
{
    public class ApplicationRepository
        : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application>
            ApplyJobAsync(Application application)
        {
            _context.Applications.Add(application);

            await _context.SaveChangesAsync();

            return application;
        }

        public async Task<IEnumerable<Application>>
            GetApplicationsByUserAsync(string userId)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>>
            GetApplicationsByJobAsync(int jobId)
        {
            return await _context.Applications
                .Include(a => a.User)
                .Where(a => a.JobId == jobId)
                .ToListAsync();
        }
        public async Task<Application?>
            GetApplicationByIdAsync(int id)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Application?>
            UpdateStatusAsync(int id, string status)
        {
            var application =
                await _context.Applications
                    .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return null;
            }

            application.Status = status;

            await _context.SaveChangesAsync();

            return application;
        }
        public async Task<IEnumerable<Application>>
    GetAllApplicationsAsync()
        {
            return await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Job)
                .ToListAsync();
        }
        public async Task<IEnumerable<Application>>
    GetApplicationsByEmployerAsync(
        string employerId)
        {
            return await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Job)
                .Where(a =>
                    a.Job.EmployerId == employerId)
                .ToListAsync();
        }
    }
}