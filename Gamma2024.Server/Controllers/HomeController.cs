using Gamma2024.Server.Models;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger; // Added for logging

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger; // Added for logging
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.EmailOuPseudo);
            if (user == null)
            {
                // Essayez de trouver l'utilisateur par son nom d'utilisateur si l'email n'a pas fonctionné
                user = await _userManager.FindByNameAsync(model.EmailOuPseudo);
                if (user == null)
                {
                    return BadRequest(new { element = "user_pseudo", message = "Aucun utilisateur trouvé" });
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
            if (result.IsLockedOut)
            {
                return BadRequest(new
                {
                    element = "lock",
                    message = "Votre compte est actuellement bloqué, " +
                    "                           veuillez contacter l'administrateur."
                });
            }
            else if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var token = GenerateJwtToken(user, roles.ToArray());

                await _signInManager.SignInAsync(user, isPersistent: false);

                return Ok(new
                {
                    message = "Connexion réussie",
                    userId = user.Id,
                    roles = roles,
                    token = token,
                    username = user.UserName,
                    name = user.Name,
                    firstName = user.FirstName,
                    photo = user.Avatar,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    pseudonym = user.UserName
                });
            }
            else
            {
                return BadRequest(new { element = "password", message = "Le mot de passe associé au compte est incorrect" });
            }
        }

        [HttpGet("check-auth")]
        [Authorize] // Cette annotation assure que seuls les utilisateurs authentifiés peuvent accéder à cette méthode
        public async Task<IActionResult> CheckAuth()
        {
            _logger.LogInformation("CheckAuth appelé");
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                _logger.LogInformation($"Utilisateur trouvé : {user.UserName}");
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(new
                {
                    isAuthenticated = true,
                    user = new
                    {
                        id = user.Id,
                        username = user.UserName,
                        email = user.Email,
                        name = user.Name,
                        firstName = user.FirstName,
                        photo = user.Avatar
                    },
                    roles = roles
                });
            }
            _logger.LogWarning("Aucun utilisateur trouvé");
            return Ok(new { isAuthenticated = false });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Déconnexion réussie" });
        }

        private string GenerateJwtToken(ApplicationUser user, string[] roles)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new("Avatar", user.Avatar ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
