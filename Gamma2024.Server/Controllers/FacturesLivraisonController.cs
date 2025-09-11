using Gamma2024.Server.Services;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturesLivraisonController : Controller
    {

        private readonly FactureService _factureService;
        private readonly FactureLivraisonService _factureLivraisonService;

        public FacturesLivraisonController(FactureService factureService, FactureLivraisonService factureLivraisonService)
        {
            _factureService = factureService;
            _factureLivraisonService = factureLivraisonService;
        }

        [HttpGet("GenererFactureLivraison/{idFacture}")]
        public IActionResult GenererFactureLivraison(int idFacture)
        {
            var facture = _factureService.ChercherFacture(idFacture);

            if (facture.IdClient != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return BadRequest("Aucun facture trouvé");
            }

            if (facture.ChoixLivraison != null)
            {
                return BadRequest("Le choix de livraison pour cette facture à déja été fait.");
            }

            var factureLivraison = _factureLivraisonService.GenererFactureLivraison(facture);

            if (factureLivraison == null)
            {
                return BadRequest("Erreur lors de la génration de la facture de livraison.");
            }
            return Ok(factureLivraison);
        }

        [HttpPost("EnregistrerChoixLivraison")]
        public async Task<IActionResult> EnregistrerChoixLivraison([FromBody] FactureLivraisonAjoutVM choixLivraison)
        {
            var factureLivraison = await _factureLivraisonService.AjouterFactureLivraison(choixLivraison);
            return Ok(factureLivraison);
        }

        [HttpGet("ChercherDetailsFactureLivraison/{idFactureLivraison}")]
        public IActionResult ChercherDetailsFactureLivraison(int idFactureLivraison)
        {
            var factureLivraison = _factureLivraisonService.ChercherDetailsFactureLivraison(idFactureLivraison);
            return Ok(factureLivraison);
        }
    }
}
