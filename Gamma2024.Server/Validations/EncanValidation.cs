using Gamma2024.Server.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace Gamma2024.Server.Validations
{
    public class EncanValidation
    {

        public static (bool IsValid, string message) ValidateEncan(EncanVM model)
        {
            if (model == null)
            {
                return (false, "Les données d'ajout d'un encan sont manquantes.");
            }

            if (model.DateDebut.ToString().IsNullOrEmpty())
            {
                return (false, "La date de début est obligatoire.");
            }

            if (model.DateDebut < DateTime.Now.AddMinutes(-1))
            {
                Console.Write(DateTime.Now.AddMinutes(-1));
                return (false, "La date de début ne peut pas être avant la date d'aujourd'hui.");
            }

            if (model.DateFin.ToString().IsNullOrEmpty())
            {
                return (false, "La date de fin est obligatoire.");
            }

            if (model.DateFin <= model.DateDebut)
            {
                return (false, "La date de fin ne doit pas être avant ou égale à la date de début.");
            }

            return (true, "L'encan a été créé.");
        }
    }
}
