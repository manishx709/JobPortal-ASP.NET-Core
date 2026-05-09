using JobPortal.API.DTOs;
using JobPortal.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace JobPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
      
        private readonly UserManager<ApplicationUser> _userManager;


        private readonly RoleManager<IdentityRole> _roleManager;

        
        private readonly IConfiguration _configuration;


        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _configuration = configuration;
        }

     
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            // Check if user already exists
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return BadRequest("User already exists.");
            }


            ApplicationUser user = new()
            {
                Email = model.Email,

                UserName = model.Email,

                FullName = model.FullName,

                SecurityStamp = Guid.NewGuid().ToString()
            };

          
            var result = await _userManager.CreateAsync(user, model.Password);

            // Check if creation failed
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Create role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(model.Role));
            }

            // Assign role to user
            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok("User registered successfully.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var isPasswordValid =
                await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),

        new Claim(JwtRegisteredClaimNames.Jti,
            Guid.NewGuid().ToString()),

        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],

                audience: _configuration["JWT:ValidAudience"],

                expires: DateTime.Now.AddHours(3),

                claims: authClaims,

                signingCredentials: new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),

                role = userRoles.FirstOrDefault(),

                expiration = token.ValidTo
            });
        }
    }
}