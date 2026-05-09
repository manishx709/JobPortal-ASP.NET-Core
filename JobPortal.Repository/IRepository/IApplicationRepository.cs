using JobPortal.Models.Entities;

namespace JobPortal.Repository.IRepository
{
    public interface IApplicationRepository
    {
        Task<Application> ApplyJobAsync(
            Application application);

        Task<IEnumerable<Application>>
            GetApplicationsByUserAsync(string userId);

        Task<IEnumerable<Application>>
            GetApplicationsByJobAsync(int jobId);

        Task<IEnumerable<Application>>
            GetAllApplicationsAsync();

        Task<Application?> GetApplicationByIdAsync(int id);

        Task<Application?> UpdateStatusAsync(
            int id,
            string status);
        Task<IEnumerable<Application>>
     GetApplicationsByEmployerAsync(
         string employerId);
    }
}