using JobPortal.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace JobPortal.Web.Controllers
{
    public class EmployerController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmployerController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        // GET CREATE JOB
        public IActionResult CreateJob()
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Employer")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            return View();
        }

        // POST CREATE JOB
        [HttpPost]
   
        public async Task<IActionResult> CreateJob(
    CreateJobDTO model)
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Employer")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            var token =
                HttpContext.Session.GetString(
                    "JWToken");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var content =
                new MultipartFormDataContent();

            content.Add(
                new StringContent(model.Title),
                "Title");

            content.Add(
                new StringContent(model.Description),
                "Description");

            content.Add(
                new StringContent(model.CompanyName),
                "CompanyName");

            content.Add(
                new StringContent(model.Location),
                "Location");

            content.Add(
                new StringContent(model.Salary.ToString()),
                "Salary");

            if (model.Logo != null)
            {
                var streamContent =
                    new StreamContent(
                        model.Logo.OpenReadStream());

                streamContent.Headers.ContentType =
                    new MediaTypeHeaderValue(
                        model.Logo.ContentType);

                content.Add(
                    streamContent,
                    "Logo",
                    model.Logo.FileName);
            }

            var response =
       await _httpClient.PostAsync(
           "https://localhost:7101/api/job/create",
           content);
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] =
                    "Job Created Successfully";

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            TempData["error"] =
                "Failed To Create Job";

            return View();
        }
        // VIEW APPLICATIONS
        public async Task<IActionResult> Applications()
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Employer")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            var token =
                HttpContext.Session.GetString(
                    "JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.GetAsync(
                    "https://localhost:7101/api/application/employer-applications");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<EmployerApplicationDTO>());
            }

            var data =
                await response.Content.ReadAsStringAsync();

            var applications =
                JsonConvert.DeserializeObject
                <List<EmployerApplicationDTO>>(data);

            return View(applications);
        }

        // ACCEPT APPLICATION
        public async Task<IActionResult> Accept(int id)
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Employer")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            var token =
                HttpContext.Session.GetString(
                    "JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            await _httpClient.PutAsync(
                $"https://localhost:7101/api/application/accept/{id}",
                null);

            TempData["success"] =
                "Application Accepted";

            return RedirectToAction(
                "Applications");
        }

        // REJECT APPLICATION
        public async Task<IActionResult> Reject(int id)
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Employer")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            var token =
                HttpContext.Session.GetString(
                    "JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            await _httpClient.PutAsync(
                $"https://localhost:7101/api/application/reject/{id}",
                null);

            TempData["success"] =
                "Application Rejected";

            return RedirectToAction(
                "Applications");
        }
    }
}