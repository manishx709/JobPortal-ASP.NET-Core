using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Repository.IRepository
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();

        Task<Job?> GetJobByIdAsync(int id);

        Task<Job> CreateJobAsync(Job job);

        Task<bool> DeleteJobAsync(int id);
        Task<IEnumerable<Job>> SearchJobsAsync(
    string? title,
    string? location,
    decimal? minSalary);
        Task<IEnumerable<Job>> GetPagedJobsAsync(
    int pageNumber,
    int pageSize);
    }
}
