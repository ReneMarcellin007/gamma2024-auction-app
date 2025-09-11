using Gamma2024.Server.ViewModels;
using System.Text.RegularExpressions;

namespace Gamma2024.Server.Validations
{
    public static class ClientValidation
    {
        public static (bool IsValid, string ErrorMessage) ValidateInscription(InscriptionVM model)
        {
            if (model == null)
            {
                return (false, "Les données d'inscription sont manquantes.");
            }

            if (model.GeneralInfo == null)
            {
                return (false, "Les informations générales sont manquantes.");
            }

            if (string.IsNullOrWhiteSpace(model.GeneralInfo.Nom))
            {
                return (false, "Le nom est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.GeneralInfo.Prenom))
            {
                return (false, "Le prénom est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.GeneralInfo.Courriel) || !IsValidEmail(model.GeneralInfo.Courriel))
            {
                return (false, "L'adresse courriel est invalide ou manquante.");
            }

            if (string.IsNullOrWhiteSpace(model.GeneralInfo.Telephone) || !IsValidPhoneNumber(model.GeneralInfo.Telephone))
            {
                return (false, "Le numéro de téléphone est invalide ou manquant.");
            }

            if (string.IsNullOrWhiteSpace(model.GeneralInfo.Pseudo))
            {
                return (false, "Le pseudo est obligatoire.");
            }

            if (model.GeneralInfo.Pseudo.Length < 3 || model.GeneralInfo.Pseudo.Length > 20)
            {
                return (false, "Le pseudo doit contenir entre 3 et 20 caractères.");
            }

            if (!Regex.IsMatch(model.GeneralInfo.Pseudo, "^[a-zA-Z0-9_]+$"))
            {
                return (false, "Le pseudo ne peut contenir que des lettres, des chiffres et des underscores.");
            }

            if (string.IsNullOrWhiteSpace(model.GeneralInfo.MotDePasse) || model.GeneralInfo.MotDePasse.Length < 8)
            {
                return (false, "Le mot de passe doit contenir au moins 8 caractères.");
            }

            if (model.GeneralInfo.MotDePasse != model.GeneralInfo.ConfirmMotPasse)
            {
                return (false, "Les mots de passe ne correspondent pas.");
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

            if (string.IsNullOrWhiteSpace(model.Adresse.CodePostal) || !IsValidPostalCode(model.Adresse.CodePostal))
            {
                return (false, "Le code postal est invalide ou manquant.");
            }

            return (true, null);
        }

        public static (bool IsValid, string ErrorMessage) ValidateClientUpdate(UpdateClientInfoVM model)
        {
            if (model == null)
            {
                return (false, "Les données de mise à jour sont manquantes.");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return (false, "Le nom est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                return (false, "Le prénom est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || !IsValidEmail(model.Email))
            {
                return (false, "L'adresse courriel est invalide ou manquante.");
            }

            if (string.IsNullOrWhiteSpace(model.PhoneNumber) || !IsValidPhoneNumber(model.PhoneNumber))
            {
                return (false, "Le numéro de téléphone est invalide ou manquant.");
            }

            if (string.IsNullOrWhiteSpace(model.Pseudonym))
            {
                return (false, "Le pseudo est obligatoire.");
            }

            if (model.Pseudonym.Length < 3 || model.Pseudonym.Length > 20)
            {
                return (false, "Le pseudo doit contenir entre 3 et 20 caractères.");
            }

            if (!Regex.IsMatch(model.Pseudonym, "^[a-zA-Z0-9_]+$"))
            {
                return (false, "Le pseudo ne peut contenir que des lettres, des chiffres et des underscores.");
            }

            if (string.IsNullOrWhiteSpace(model.CivicNumber))
            {
                return (false, "Le numéro civique est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Street))
            {
                return (false, "La rue est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.City))
            {
                return (false, "La ville est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Province))
            {
                return (false, "La province est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.Country))
            {
                return (false, "Le pays est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(model.PostalCode) || !IsValidPostalCode(model.PostalCode))
            {
                return (false, "Le code postal est invalide ou manquant.");
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    return (false, "Le mot de passe actuel est requis pour changer le mot de passe.");
                }

                if (model.NewPassword.Length < 8)
                {
                    return (false, "Le nouveau mot de passe doit contenir au moins 8 caractères.");
                }

                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    return (false, "Les mots de passe ne correspondent pas.");
                }
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
            return Regex.IsMatch(phoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        }

        private static bool IsValidCreditCardNumber(string cardNumber)
        {
            return Regex.IsMatch(cardNumber, @"^(?:4[0-9]{15}|4[0-9]{3} [0-9]{4} [0-9]{4} [0-9]{4}|5[1-5][0-9]{14}|5[1-5][0-9]{2} [0-9]{4} [0-9]{4} [0-9]{4}|6(?:011|5[0-9]{2})[0-9]{12}|6(?:011|5[0-9]{2}) [0-9]{4} [0-9]{4} [0-9]{4}|3[47][0-9]{13}|3[47][0-9]{2} [0-9]{6} [0-9]{5}|3(?:0[0-5]|[68][0-9])[0-9]{11}|3(?:0[0-5]|[68][0-9]) [0-9]{4} [0-9]{6} [0-9]{4}|(?:2131|1800|35\d{3})[0-9]{12}|(?:2131|1800|35\d{3}) [0-9]{4} [0-9]{4} [0-9]{4})$");

        }

        private static bool IsValidPostalCode(string postalCode)
        {
            return Regex.IsMatch(postalCode,
                @"^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvwxyz]\d[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz][ -]?\d[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz]\d$",
                RegexOptions.IgnoreCase);
        }
    }
}