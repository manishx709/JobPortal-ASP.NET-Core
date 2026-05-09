using JobPortal.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace JobPortal.Web.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly HttpClient _httpClient;

        public ApplicationController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        // GET APPLY PAGE
        public IActionResult Apply(int jobId)
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Candidate")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            ViewBag.JobId = jobId;

            return View();
        }

        // POST APPLY
        [HttpPost]
        public async Task<IActionResult> Apply(
            int jobId,
            IFormFile resume)
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Candidate")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            if (resume == null || resume.Length == 0)
            {
                TempData["error"] =
                    "Please upload resume";

                return View();
            }

            var content =
                new MultipartFormDataContent();

            var streamContent =
                new StreamContent(
                    resume.OpenReadStream());

            streamContent.Headers.ContentType =
                new MediaTypeHeaderValue(
                    resume.ContentType);

            content.Add(
                streamContent,
                "resume",
                resume.FileName);

            var token =
                HttpContext.Session.GetString(
                    "JWToken");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.PostAsync(
                    $"https://localhost:7101/api/application/apply/{jobId}",
                    content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] =
                    "Application Submitted Successfully";

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            TempData["error"] =
                "Application Failed";

            return View();
        }

        // MY APPLICATIONS
        public async Task<IActionResult> MyApplications()
        {
            var role =
                HttpContext.Session.GetString(
                    "UserRole");

            if (role != "Candidate")
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

            var response =
                await _httpClient.GetAsync(
                    "https://localhost:7101/api/application/my-applications");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<ApplicationDTO>());
            }

            var data =
                await response.Content.ReadAsStringAsync();

            var applications =
                JsonConvert.DeserializeObject
                <List<ApplicationDTO>>(data);

            return View(applications);
        }
    }
}