using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Gamma2024.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gamma2024.Server.Controllers
{
    [Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
    [Route("api/[controller]")]
    public class AdministrateurController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AdministrateurService _administrateurService;


        public AdministrateurController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, AdministrateurService administrateurService)
        {
            _userManager = userManager;
            _context = context;
            _administrateurService = administrateurService;
        }


        [HttpGet("ObtenirTousLesUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _administrateurService.TousLesMembre();

            if (users == null)
            {
                return NotFound("Liste d'utilisateur vide");
            }
            return Ok(users);
        }


        [HttpGet("obtenirTousLesMembres/{membreId}")]
        public async Task<IActionResult> GetUserById(string membreId)
        {
            var user = await _administrateurService.ObtenirMembreParId(membreId);

            if (user == null)
            {
                return NotFound("Aucun utilisateur trouvé");
            }
            return Ok(user);
        }


        [HttpGet("bloquerMembre/{membreId}")]
        public async Task<IActionResult> LockMember(string membreId)
        {
            var user = await _userManager.FindByIdAsync(membreId);

            if (user == null)
            {
                return NotFound("Aucun membre trouvé");
            }

            var result1 = await _userManager.SetLockoutEnabledAsync(user, true);

            var result2 = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(1000));

            if (!result1.Succeeded || !result2.Succeeded)
            {
                return BadRequest("Utilisateur non-bloqué");
            }
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { etat = true, message = "Membre bloqué avec succès" });
        }


        [HttpGet("debloquerMembre/{membreId}")]
        public async Task<IActionResult> UnLockMember(string membreId)
        {
            var user = await _userManager.FindByIdAsync(membreId);

            if (user == null)
            {
                return NotFound("Aucun membre trouvé");
            }

            if (!await _userManager.GetLockoutEnabledAsync(user))
            {
                var enableLockoutResult = await _userManager.SetLockoutEnabledAsync(user, true);
                if (!enableLockoutResult.Succeeded)
                {
                    return BadRequest("Erreur lors de l'activation du verrouillage");
                }

                var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(1));
                if (!lockoutResult.Succeeded)
                {
                    return BadRequest("Erreur lors du verrouillage");
                }
            }

            var result2 = await _userManager.SetLockoutEndDateAsync(user, null);
            var result1 = await _userManager.SetLockoutEnabledAsync(user, false);
            if (!result1.Succeeded || !result2.Succeeded)

            {
                return BadRequest("Erreur lors du verrouillage ");
            }

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { etat = false, message = "Membre débloqué avec succès" });
        }


    }
}
