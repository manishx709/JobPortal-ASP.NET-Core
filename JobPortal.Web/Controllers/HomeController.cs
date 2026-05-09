using JobPortal.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JobPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(
     string search,
     string location)
        {
            var response =
                await _httpClient.GetAsync(
                    "https://localhost:7101/api/job");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<JobDTO>());
            }

            var data =
                await response.Content.ReadAsStringAsync();

            var jobs =
                JsonConvert.DeserializeObject
                <List<JobDTO>>(data);

            if (!string.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(j =>

                    j.Title.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    j.CompanyName.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase))

                    .ToList();
            }

            if (!string.IsNullOrEmpty(location))
            {
                jobs = jobs.Where(j =>

                    j.Location.Contains(
                        location,
                        StringComparison.OrdinalIgnoreCase))

                    .ToList();
            }

            return View(jobs);
        }
        public async Task<IActionResult> Details(int id)
        {
            var response =
                await _httpClient.GetAsync(
                    $"https://localhost:7101/api/job/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var data =
                await response.Content.ReadAsStringAsync();

            var job =
                JsonConvert.DeserializeObject<JobDTO>(data);

            return View(job);
        }
    }
}