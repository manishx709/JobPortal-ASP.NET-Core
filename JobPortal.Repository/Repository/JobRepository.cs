using JobPortal.Data;
using JobPortal.Models.Entities;
using JobPortal.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repository.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Job?> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task<Job> CreateJobAsync(Job job)
        {
            _context.Jobs.Add(job);

            await _context.SaveChangesAsync();

            return job;
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return false;
            }

            _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Job>>
    SearchJobsAsync(
        string? title,
        string? location,
        decimal? minSalary)
        {
            var query = _context.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(j =>
                    j.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j =>
                    j.Location.Contains(location));
            }

            if (minSalary.HasValue)
            {
                query = query.Where(j =>
                    j.Salary >= minSalary.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Job>>
    GetPagedJobsAsync(
        int pageNumber,
        int pageSize)
        {
            return await _context.Jobs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}