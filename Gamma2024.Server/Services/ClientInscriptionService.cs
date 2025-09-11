using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Gamma2024.Server.Validations;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Gamma2024.Server.Services
{
    public class ClientInscriptionService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomerService _customerService;

        public ClientInscriptionService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _customerService = new CustomerService();
        }

        public async Task<(bool Success, string Message)> InscrireUtilisateur(InscriptionVM model)
        {
            // Vérification des données
            var (isValid, errorMessage) = ClientValidation.ValidateInscription(model);
            if (!isValid)
            {
                return (false, errorMessage);
            }

            // Vérifier l'unicité du pseudonyme
            var existingUserByUsername = await _userManager.FindByNameAsync(model.GeneralInfo.Pseudo);
            if (existingUserByUsername != null)
            {
                return (false, "Ce pseudonyme est déjà utilisé. Veuillez en choisir un autre.");
            }

            // Vérifier l'unicité de l'email
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.GeneralInfo.Courriel);
            if (existingUserByEmail != null)
            {
                return (false, "Cette adresse email est déjà utilisée.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var options = new CustomerCreateOptions()
                {
                    Email = model.GeneralInfo.Courriel,
                    Name = model.GeneralInfo.Prenom + " " + model.GeneralInfo.Nom,
                    Description = model.GeneralInfo.Pseudo,
                    PreferredLocales = ["fr-CA"]
                };
                var customer = _customerService.Create(options);

                // Créer le client (ApplicationUser)
                var client = new ApplicationUser
                {
                    UserName = model.GeneralInfo.Pseudo,
                    NormalizedUserName = model.GeneralInfo.Pseudo.ToUpper(),
                    Email = model.GeneralInfo.Courriel,
                    EmailConfirmed = false,
                    NormalizedEmail = model.GeneralInfo.Courriel.ToUpper(),
                    Name = model.GeneralInfo.Nom,
                    FirstName = model.GeneralInfo.Prenom,
                    PhoneNumber = model.GeneralInfo.Telephone,
                    PhoneNumberConfirmed = true,
                    Avatar = "/Avatars/Defaut.png",
                    StripeCustomer = customer.Id
                };

                var result = await _userManager.CreateAsync(client, model.GeneralInfo.MotDePasse);

                if (!result.Succeeded)
                {
                    _customerService.Delete(customer.Id);
                    return (false, "Erreur lors de la création de l'utilisateur : " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // Ajouter le rôle CLIENT
                await _userManager.AddToRoleAsync(client, "CLIENT");

                // Créer l'adresse
                var adresse = new Models.Adresse
                {
                    Numero = int.Parse(model.Adresse.NumeroCivique),
                    Rue = model.Adresse.Rue,
                    Appartement = model.Adresse.Appartement,
                    Ville = model.Adresse.Ville,
                    Province = model.Adresse.Province,
                    Pays = model.Adresse.Pays,
                    CodePostal = ParseCodePostal(model.Adresse.CodePostal),
                    EstDomicile = true,
                    IdApplicationUser = client.Id,

                };
                _context.Adresses.Add(adresse);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Inscription réussie");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, "Une erreur est survenue lors de l'inscription : " + ex.Message);
            }
        }

        private async Task<bool> IsUsernameUnique(string username)
        {
            return !await _context.Users.AnyAsync(u => u.UserName == username);
        }

        public static string ParseCodePostal(string code)
        {
            var parts = code.Split(' ');
            return parts[0] + parts[1];
        }
    }
}