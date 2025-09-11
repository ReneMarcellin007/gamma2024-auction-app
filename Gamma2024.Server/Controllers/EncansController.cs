using Gamma2024.Server.Data;
using Gamma2024.Server.Services;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncansController : ControllerBase
    {
        private readonly EncanService _encanService;
        private readonly ApplicationDbContext _context;

        public EncansController(EncanService encanService, ApplicationDbContext context)
        {
            _encanService = encanService;
            _context = context;
        }

        [HttpGet("cherchertousencans")]
        public ICollection<EncanAffichageAdminVM> ChercherTousEncans()
        {
            ICollection<EncanAffichageAdminVM> encans = _encanService.ChercherTousEncans();
            return encans;
        }

        [HttpGet("ChercherTousEncansVisibles")]
        public ICollection<EncanAffichageAdminVM> ChercherTousEncansVisibles()
        {
            ICollection<EncanAffichageAdminVM> encans = _encanService.ChercherTousEncansVisibles();
            return encans;
        }

        [HttpPost("creerEncan")]
        public async Task<IActionResult> CreerEncan([FromBody] EncanVM vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (sucess, message) = await _encanService.CreerEncan(vm);
            if (sucess)
            {
                return Ok(new { sucess = true, message = message });
            }
            else
            {
                return BadRequest(new { sucess = false, message = message });
            }
        }

        [HttpDelete("supprimerEncan/{numeroEncan}")]
        public IActionResult SupprimerEncan(int numeroEncan)
        {
            var encan = _encanService.GetEncanByNumber(numeroEncan);
            if (encan == null)
            {
                return BadRequest(new { sucess = false, message = "Aucun encan trouvé" });
            }

            _context.Encans.Remove(encan);
            _context.SaveChanges();

            return Ok(new { sucess = true, message = "Encan supprimé avec succès" });
        }



        [HttpGet("obtenirUnEncan/{idEncan}")]
        public IActionResult ObtenirEncan(int idEncan)
        {
            var encan = _encanService.GetEncanById(idEncan);
            if (encan == null)
            {
                return BadRequest(new { sucess = false, message = "Aucun encan trouvé" });
            }

            return Ok(encan);
        }

        [HttpPut("modifierEncan/{id}")]
        public async Task<IActionResult> Modifier(int id, [FromBody] EncanVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message) = await _encanService.ModifierEncan(id, model);
            if (success)
            {
                return Ok(new { success = true, message = message });
            }
            else
            {
                return BadRequest(new { success = false, message = message });
            }
        }

        [HttpPut("mettreAJourEncanPublie")]
        public async Task<IActionResult> MettreAJourEncanPublie([FromBody] EncanPublieVM vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (sucess, message) = await _encanService.MettreAJourEncanPublie(vm);
            if (sucess)
            {
                return Ok(new { sucess = true, message = message });
            }
            else
            {
                return BadRequest(new { sucess = false, message = message });
            }
        }

        [HttpGet("ChercherEncanParNumero/{numeroEncan}")]
        public EncanAffichageVM ChercherEncanParNumero(string numeroEncan)
        {
            try
            {
                var numeroEncanInt = int.Parse(numeroEncan);
                var encan = _encanService.ChercherEncanParNumero(numeroEncanInt);
                return encan;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("ChercherEncanEnCours")]
        public EncanAffichageVM ChercherEncanEnCours()
        {
            try
            {
                var encan = _encanService.ChercherEncanEnCours();
                return encan;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("ChercherNumeroEncanEnCours")]
        public int ChercherNumeroEncanEnCours()
        {
            try
            {
                var numero = _encanService.ChercherNumeroEncanEnCours();
                return numero;
            }
            catch
            {
                return 0;
            }
        }

        [HttpGet("ChercherEncansFuturs")]
        public ICollection<EncanAffichageVM> ChercherEncansFuturs()
        {
            try
            {
                var encans = _encanService.ChercherEncansFuturs();
                return encans;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("ChercherEncansPasses")]
        public ICollection<EncanAffichageVM> ChercherEncansPasses()
        {
            try
            {
                var encans = _encanService.ChercherEncansPasses();
                return encans;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("etat-courant")]
        public async Task<IActionResult> GetEtatCourant()
        {
            var (type, encan) = await _encanService.GetEtatCourant();
            return Ok(new { type, encan });
        }
    }
}
