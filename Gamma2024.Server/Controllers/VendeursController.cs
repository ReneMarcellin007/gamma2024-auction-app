using Gamma2024.Server.Models;
using Gamma2024.Server.Services;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
    public class VendeursController : ControllerBase
    {
        private readonly VendeurService _vendeurService;

        public VendeursController(VendeurService vendeurService)
        {
            _vendeurService = vendeurService;
        }

        [HttpPost("creer")]
        public async Task<IActionResult> Creer([FromBody] VendeurVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = "Validation failed", errors = errors });
            }

            try
            {
                var (success, message) = await _vendeurService.CreerVendeur(model);
                if (success)
                {
                    return Ok(new { success = true, message = message });
                }
                else
                {
                    return BadRequest(new { success = false, message = message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Une erreur interne s'est produite", details = ex.Message });
            }
        }

        [HttpPut("modifier/{id}")]
        public async Task<IActionResult> Modifier(int id, [FromBody] VendeurVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message) = await _vendeurService.ModifierVendeur(id, model);
            if (success)
            {
                return Ok(new { success = true, message = message });
            }
            else
            {
                return BadRequest(new { success = false, message = message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtenir(int id)
        {
            var vendeur = await _vendeurService.ObtenirVendeur(id);
            if (vendeur == null)
            {
                return NotFound();
            }
            return Ok(vendeur);
        }

        [HttpGet]
        [HttpGet("tous")]
        public async Task<IActionResult> ObtenirTous()
        {
            var vendeurs = await _vendeurService.ObtenirTousVendeurs();
            return Ok(vendeurs);
        }
    }
}