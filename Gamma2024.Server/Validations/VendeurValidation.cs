using Gamma2024.Server.ViewModels;
using System.Text.RegularExpressions;

namespace Gamma2024.Server.Validations
{
    public static class VendeurValidation
    {
        public static (bool IsValid, string ErrorMessage) ValidateVendeur(VendeurVM model)
        {
            if (model == null)
            {
                return (false, "Les données du vendeur sont manquantes.");
            }

            if (string.IsNullOrWhiteSpace(model.Nom))
            {
                return (false, "Le nom est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Prenom))
            {
                return (false, "Le prénom est obligatoire.");
            }

            if (!IsValidEmail(model.Courriel))
            {
                return (false, "L'adresse courriel est invalide ou manquante.");
            }

            if (!IsValidPhoneNumber(model.Telephone))
            {
                return (false, "Le numéro de téléphone est invalide ou manquant.");
            }

            if (model.Adresse == null)
            {
                return (false, "Les informations d'adresse sont manquantes.");
            }

            if (string.IsNullOrWhiteSpace(model.Adresse.NumeroCivique))
            {
                return (false, "Le numéro civique est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Adresse.Rue))
            {
                return (false, "La rue est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Adresse.Ville))
            {
                return (false, "La ville est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Adresse.Province))
            {
                return (false, "La province est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Adresse.Pays))
            {
                return (false, "Le pays est obligatoire.");
            }

            model.Adresse.CodePostal = NormalizePostalCode(model.Adresse.CodePostal);

            if (!IsValidPostalCode(model.Adresse.CodePostal))
            {
                return (false, "Le code postal est invalide ou manquant.");
            }

            return (true, null);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\d{3}-\d{3}-\d{4}$");
        }

        private static bool IsValidPostalCode(string postalCode)
        {
            return Regex.IsMatch(postalCode,
                @"^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvwxyz]\d[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz][ -]?\d[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz]\d$",
                RegexOptions.IgnoreCase);
        }

        private static string NormalizePostalCode(string postalCode)
        {
            return postalCode.ToUpper().Replace(" ", "");
        }
    }
}