using Gamma2024.Server.Data;
using Gamma2024.Server.Hub;
using Gamma2024.Server.Models;
using Gamma2024.Server.Services;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotsController : ControllerBase
    {
        private readonly LotService _lotService;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<LotMiseHub> _hubContext;
		private readonly ILogger<LotsController> _logger;


        public LotsController(LotService lotService, ApplicationDbContext context, IHubContext<LotMiseHub> hubContext, ILogger<LotsController> logger)
        {
            _lotService = lotService;
            _context = context;
            _hubContext = hubContext;
			_logger = logger;
        }

		[Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
		[HttpGet("tous")]
		public async Task<ActionResult<IEnumerable<LotAffichageVM>>> ObtenirTousLots()
		{
			var lots = await _lotService.ObtenirTousLots();
			return Ok(lots.ToList());
		}

		[Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
		[HttpGet("{id}")]
		public async Task<ActionResult<LotAffichageVM>> ObtenirLot(int id)
		{
			var lot = await _lotService.ObtenirLot(id);
			if (lot == null)
			{
				return NotFound();
			}
			return Ok(lot);
		}

		[Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
		[HttpPost("creer")]
		public async Task<ActionResult<LotAffichageVM>> CreerLot([FromForm] LotCreationVM lotVM)
		{
			if (Request.Form.Files.Count > 0)
			{
				lotVM.Photos = Request.Form.Files.ToList();
			}

			var resultat = await _lotService.CreerLot(lotVM);
			if (!resultat.Success)
			{
				return BadRequest(resultat.Message);
			}
			return Ok(new { success = true, message = "Lot crée avec succès" });
		}

		[Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
		[HttpPut("modifier/{id}")]
		public async Task<IActionResult> ModifierLot(int id, LotModificationVM lotVM)
		{
			var resultat = await _lotService.ModifierLot(id, lotVM);
			if (!resultat.Success)
			{
				return BadRequest(resultat.Message);
			}
			return Ok(new { success = true, message = "Lot modifié avec succès" });
		}

		[Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
		[HttpDelete("supprimer/{id}")]
		public async Task<IActionResult> SupprimerLot(int id)
		{
			var resultat = await _lotService.SupprimerLot(id);
			if (!resultat.Success)
			{
				return BadRequest(resultat.Message);
			}
			return Ok(new { success = true, message = "Lot supprimé avec succès" });
		}

		[HttpGet("artistes")]
		public async Task<ActionResult<ICollection<ArtisteVM>>> ObtenirTousArtistes()
		{
			try
			{
				var artistes = _lotService.ObtenirTousArtistes();
				return Ok(artistes);
			}
			catch (Exception ex)
			{
				return BadRequest($"Erreur d'obtenir tous les artistes: {ex.Message}");
			}
		}

		[HttpGet("categories")]
		public async Task<ActionResult<IEnumerable<Categorie>>> ObtenirCategories()
		{
			try 
			{
				var categories = await _context.Categories.ToListAsync();
				return Ok(categories);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erreur lors de la récupération des catégories");
				return StatusCode(500, "Erreur lors de la récupération des catégories");
			}
		}

		[Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
		[HttpGet("vendeurs")]
		public async Task<ActionResult<IEnumerable<Vendeur>>> ObtenirVendeurs()
		{
			try 
			{
				var vendeurs = await _context.Vendeurs.ToListAsync();
				return Ok(vendeurs);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erreur lors de la récupération des vendeurs");
				return StatusCode(500, "Erreur lors de la récupération des vendeurs");
			}
		}

		[HttpGet("mediums")]
		public async Task<ActionResult<IEnumerable<Medium>>> ObtenirMediums()
		{
			try 
			{
				var mediums = await _context.Mediums.ToListAsync();
				return Ok(mediums);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erreur lors de la récupération des mediums");
				return StatusCode(500, "Erreur lors de la récupération des mediums");
			}
		}

		[Authorize(Roles = ApplicationRoles.ADMINISTRATEUR)]
		[HttpGet("encans")]
		public async Task<ActionResult<IEnumerable<Encan>>> ObtenirEncans()
		{
			try 
			{
				var encans = await _context.Encans.ToListAsync();
				return Ok(encans);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erreur lors de la récupération des encans");
				return StatusCode(500, "Erreur lors de la récupération des encans");
			}
		}

		[HttpGet("cherchertouslotsrecherche")]
		[AllowAnonymous]
		public async Task<ActionResult<ICollection<LotEncanAffichageVM>>> ChercherTousLotsRecherche()
		{
			try
			{
				var lots = _lotService.ChercherTousLotsRecherche();
				return Ok(lots);
			}
			catch (Exception ex)
			{
				return BadRequest($"Erreur: {ex.Message}");
			}
		}

		[HttpGet("cherchertouslotsparencan/{idEncan}")]
		[AllowAnonymous]
		public async Task<ActionResult<ICollection<LotEncanAffichageVM>>> ChercherTousLotsParEncan(string idEncan)
		{
			try
			{
				var idEncanInt = int.Parse(idEncan);
				var lots = _lotService.ChercherTousLotsParEncan(idEncanInt);
				return Ok(lots);
			}
			catch (Exception ex)
			{
				return BadRequest($"Erreur: {ex.Message}");
			}
		}

		[AllowAnonymous]
		[HttpGet("chercherDetailsLotParId/{idLot}")]
		public LotDetailsVM ChercherDetailsLotParId(string idLot)
		{
			try
			{
				var idLotInt = int.Parse(idLot);
				LotDetailsVM lot = _lotService.ChercherDetailsLotParId(idLotInt);
				return lot;
			}
			catch
			{
				return null;
			}
		}

		[Authorize(Roles = ApplicationRoles.CLIENT)]
		[HttpPost("placerMise")]
		public async Task<IActionResult> PlacerMise([FromBody] MiseVM mise)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			mise.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(mise.UserId))
			{
				return Unauthorized();
			}

			(bool isSuccess, string resultMessage) = await _lotService.PlacerMise(mise);

			if (isSuccess)
			{
				var lastUserBid = await _lotService.GetUserLastBid(mise.LotId, mise.UserId);

				return Ok(new
				{
					success = true,
					message = resultMessage,
					userLastBid = lastUserBid
				});
			}
			else
			{
				return BadRequest(new
				{
					success = false,
					message = resultMessage
				});
			}
		}

		[Authorize]
		[HttpGet("userBids/{userId}")]
		public async Task<IActionResult> GetUserBids(string userId)
		{
			try
			{
				_logger.LogInformation($"Contrôleur - Début GetUserBids pour userId: {userId}");
				
				var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				_logger.LogInformation($"Contrôleur - CurrentUserId: {currentUserId}, RequestedUserId: {userId}");

				if (currentUserId != userId)
				{
					_logger.LogWarning($"Contrôleur - Accès non autorisé: currentUserId ({currentUserId}) != requestedUserId ({userId})");
					return Forbid();
				}

				var userBids = await _lotService.GetUserBids(userId);
				return Ok(userBids);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Contrôleur - Erreur dans GetUserBids pour userId: {userId}");
				return StatusCode(500, "Une erreur est survenue lors de la récupération des mises");
			}
		}

		[Authorize]
		[HttpGet("userLastBid/{lotId}")]
		public async Task<ActionResult<double?>> GetUserLastBid(int lotId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var lastBid = await _lotService.GetUserLastBid(lotId, userId);
			return Ok(lastBid);
		}

		[Authorize]
		[HttpGet("userBidsGroupedByEncan")]
		public async Task<IActionResult> GetUserBidsGroupedByEncan()
		{
			try
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (string.IsNullOrEmpty(userId))
				{
					return Unauthorized();
				}

				var misesParEncan = await _lotService.GetUserBidsGroupedByEncan(userId);
				return Ok(misesParEncan);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erreur lors de la récupération des mises groupées par encan");
				return StatusCode(500, "Une erreur est survenue");
			}
		}


    }
}
