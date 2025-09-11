using Gamma2024.Server.Data;
using Gamma2024.Server.Interface;
using Gamma2024.Server.Models;
using Gamma2024.Server.Services;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UtilisateursController : ControllerBase
    {
        private readonly ClientInscriptionService _inscriptionService;
        private readonly ClientModificationService _modificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        public UtilisateursController(ClientInscriptionService inscriptionService, IEmailSender emailSender,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context, ClientModificationService modificationService,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _inscriptionService = inscriptionService;
            _emailSender = emailSender;
            _userManager = userManager;
            _context = context;
            _modificationService = modificationService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("creer")]
        public async Task<IActionResult> Creer([FromBody] InscriptionVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message) = await _inscriptionService.InscrireUtilisateur(model);
            if (success)
            {

                var client = await _userManager.FindByEmailAsync(model.GeneralInfo.Courriel);
                if (client != null)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(client);
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    // Construire le lien de confirmation
                    var confirmationLink = Url.Action("ConfirmEmail", "Utilisateurs", new { userId = client.Id, token }, protocol: Request.Scheme);

                    var subject = "Confirmation de courriel";
                    var messageText = $"Veuillez confirmer votre courriel en cliquant sur ce lien : <a href='{confirmationLink}'>Confirmer votre compte</a>";

                    _emailSender.Send(client.Email, subject, messageText);

                }

                return Ok(new { success = true, message = message });
            }
            else
            {
                return BadRequest(new { success = false, message = message });
            }

        }



        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery(Name = "userId")] string idClient, string token)
        {
            if (string.IsNullOrEmpty(idClient) || string.IsNullOrEmpty(token))
            {
                return BadRequest("User ID ou Token manquant");
            }

            var client = await _userManager.FindByIdAsync(idClient);
            if (client == null)
            {
                return NotFound("Utilisateur introuvable");
            }


            client.EmailConfirmed = true;

            _context.Users.Update(client);
            await _context.SaveChangesAsync();


            return Ok("Email confirmé avec succès.");
        }


        [Authorize(Roles = "Client")]
        [HttpGet("ObtentionInfoClient")]
        public async Task<IActionResult> GetClientInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("ID utilisateur non trouvé");
            }

            var user = await _userManager.Users
                .OfType<ApplicationUser>()
                .Include(u => u.Adresses.Where(a => a.EstDomicile))
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("Utilisateur non trouvé.");
            }

            var clientInfo = new
            {
                id = user.Id,
                username = user.UserName,
                name = user.Name,
                firstName = user.FirstName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                pseudonym = user.UserName,
                photo = string.IsNullOrEmpty(user.Avatar) ? "/Avatars/Defaut.png" : user.Avatar,
                roles = await _userManager.GetRolesAsync(user),
                civicNumber = user.Adresses.FirstOrDefault()?.Numero.ToString(),
                street = user.Adresses.FirstOrDefault()?.Rue,
                apartment = user.Adresses.FirstOrDefault()?.Appartement,
                city = user.Adresses.FirstOrDefault()?.Ville,
                province = user.Adresses.FirstOrDefault()?.Province,
                country = user.Adresses.FirstOrDefault()?.Pays,
                postalCode = user.Adresses.FirstOrDefault()?.CodePostal
            };

            return Ok(clientInfo);
        }

        [Authorize(Roles = "Client")]
        [HttpPut("miseajourinfoclient")]
        public async Task<IActionResult> UpdateClientInfo([FromBody] UpdateClientInfoVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (success, message, updatedUser) = await _modificationService.MettreAJourClient(userId, model);

            if (success)
            {
                return Ok(new { success = true, message = message, user = updatedUser });
            }
            else
            {
                return BadRequest(new { success = false, message = message });
            }
        }

        [Authorize(Roles = "Client")]
        [HttpPut("avatar")]
        public async Task<IActionResult> UpdateAvatar(IFormFile avatar)
        {
            if (avatar == null || avatar.Length == 0)
            {
                return BadRequest("Aucun fichier n'a été envoyé ou le fichier est vide.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("ID utilisateur non trouvé");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Utilisateur non trouvé.");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Avatars");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + avatar.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await avatar.CopyToAsync(fileStream);
            }

            user.Avatar = $"/Avatars/{uniqueFileName}";
            await _userManager.UpdateAsync(user);

            return Ok(new { avatarUrl = user.Avatar });
        }

        [HttpGet("verifier-email")]
        public async Task<IActionResult> VerifierEmail([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var disponible = user == null;
            Console.WriteLine($"Vérification de l'email '{email}': disponible = {disponible}");
            return Ok(new { disponible = disponible });
        }

        [HttpGet("verifier-pseudonyme")]
        public async Task<IActionResult> VerifierPseudonyme([FromQuery] string pseudo)
        {
            if (string.IsNullOrWhiteSpace(pseudo))
            {
                return BadRequest("Le pseudonyme ne peut pas être vide.");
            }

            var utilisateurExistant = await _userManager.FindByNameAsync(pseudo);
            var disponible = utilisateurExistant == null;
            return Ok(new { disponible = disponible });
        }



        [HttpPost("reinitialiserMotDePasse")]
        public async Task<IActionResult> reinitialiserMotDePasse([FromBody] ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (model is null)
                {
                    return BadRequest("Aucune donnée envoyé");
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest("Aucun utilisateur trouvé");
                }

                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (string.IsNullOrEmpty(resetToken))
                {
                    return BadRequest("Erreur lors de la génération du token");
                }

                var result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

                if (result.Succeeded)
                {
                    return Ok("Mot de passe changé avec succès");
                }
                else
                {
                    return BadRequest("Le mot de passe doit avoir au moins 1 majuscule, 1 minuscule, 1 chiffre et 1 caractère special");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("ChercherAdressesClient")]
        public async Task<List<AdresseInfoVM>> ChercherAdressesClient()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _userManager.Users
                .OfType<ApplicationUser>()
                .Include(u => u.Adresses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user.Adresses.Select(a => new AdresseInfoVM
            {
                Appartement = a.Appartement,
                CodePostal = a.CodePostal,
                NumeroCivique = a.Numero,
                Pays = a.Pays,
                Province = a.Province,
                Rue = a.Rue,
                Ville = a.Ville,
                Id = a.Id
            }).ToList();
        }
    }

}
