using JobPortal.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace JobPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var json =
                JsonConvert.SerializeObject(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    "https://localhost:7101/api/auth/login",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error =
                    "Invalid Login";

                return View();
            }

            var data =
             await response.Content.ReadAsStringAsync();

            var loginResponse =
                JsonConvert.DeserializeObject
                <LoginResponseDTO>(data);

            HttpContext.Session.SetString(
                "JWToken",
                loginResponse.Token);

            HttpContext.Session.SetString(
                "UserRole",
                loginResponse.Role);




            return RedirectToAction(
                "Index",
                "Home");
        }

        // REGISTER PAGE
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER POST
        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterDTO model)
        {
            var json =
                JsonConvert.SerializeObject(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    "https://localhost:7101/api/auth/register",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Login");
            }

            ViewBag.Error =
                "Registration Failed";

            return View();
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login");
        }
    }
}