using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Gamma2024.Server.Validations;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Services
{
    public class ClientModificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientModificationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool Success, string Message, object UpdatedUser)> MettreAJourClient(string userId, UpdateClientInfoVM model)
        {
            // Vérification des données
            var (isValid, errorMessage) = ClientValidation.ValidateClientUpdate(model);
            if (!isValid)
            {
                return (false, errorMessage, new object());
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "Utilisateur non trouvé.", new object());
            }

            // Mettre à jour les informations générales
            user.Name = model.Name;
            user.FirstName = model.FirstName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Pseudonym;

            // Mettre à jour les informations d'adresse
            var adresse = await _context.Adresses.FirstOrDefaultAsync(a => a.IdApplicationUser == user.Id && a.EstDomicile);
            if (adresse != null)
            {
                if (!int.TryParse(model.CivicNumber, out int numeroCivique))
                {
                    return (false, "Le numéro civique doit être un nombre entier.", new object());
                }
                adresse.Numero = numeroCivique;
                adresse.Rue = model.Street;
                adresse.Appartement = model.Apartment;
                adresse.Ville = model.City;
                adresse.Province = model.Province;
                adresse.Pays = model.Country;
                adresse.CodePostal = model.PostalCode;
            }
            else
            {
                return (false, "Aucune adresse trouvée pour cet utilisateur.", new object());
            }

            // Gestion du mot de passe
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    return (false, "Le mot de passe actuel est requis pour changer le mot de passe.", new object());
                }

                var passwordCheckResult = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!passwordCheckResult)
                {
                    return (false, "Mot de passe actuel incorrect.", new object());
                }

                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);

                if (!passwordValidationResult.Succeeded)
                {
                    return (false, string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description)), new object());
                }

                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    return (false, "Erreur lors de la mise à jour du mot de passe : " + string.Join(", ", passwordChangeResult.Errors.Select(e => e.Description)), new object());
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return (false, "Erreur lors de la mise à jour des informations : " + string.Join(", ", result.Errors.Select(e => e.Description)), new object());
            }

            await _context.SaveChangesAsync();

            var updatedUser = new
            {
                user.Name,
                user.FirstName,
                user.Email,
                user.PhoneNumber,
                user.UserName,
                CivicNumber = adresse.Numero.ToString(),
                Street = adresse.Rue,
                Apartment = adresse.Appartement,
                City = adresse.Ville,
                Province = adresse.Province,
                Country = adresse.Pays,
                PostalCode = adresse.CodePostal
            };

            return (true, "Informations mises à jour avec succès.", updatedUser);
        }
    }
}