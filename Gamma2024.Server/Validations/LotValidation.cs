using Gamma2024.Server.Data;
using Gamma2024.Server.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Validations
{
    public static class LotValidation
    {
        public static async Task<(bool IsValid, string ErrorMessage)> ValidateLot<T>(T lot, ApplicationDbContext context) where T : LotCreationVM
        {
            // Vérification du numéro de lot unique dans le même encan
            var query = context.Lots
                .Include(l => l.EncanLots)
                .Where(l => l.Numero == lot.Numero);

            // Si c'est une modification, exclure le lot actuel de la vérification
            //if (lot is LotModificationVM modificationLot)
            //{
            //    query = query.Where(l => l.Id != modificationLot.Id);

            //    // Vérifier dans l'encan cible
            //    var lotExistant = await query.AnyAsync(l =>
            //        l.EncanLots.Any(el => el.IdEncan == modificationLot.IdEncanModifie));

            //    if (lotExistant)
            //    {
            //        return (false, "Un lot avec ce numéro existe déjà dans l'encan cible.");
            //    }
            //}

            if (string.IsNullOrWhiteSpace(lot.Description))
            {
                return (false, "La description du lot est obligatoire.");
            }

            if (lot.ValeurEstimeMin <= 0)
            {
                return (false, "La valeur estimée minimale doit être supérieure à zéro.");
            }

            if (lot.ValeurEstimeMax <= lot.ValeurEstimeMin)
            {
                return (false, "La valeur estimée maximale doit être supérieure à la valeur minimale.");
            }

            if (lot.PrixOuverture <= 0)
            {
                return (false, "Le prix d'ouverture doit être supérieur à zéro.");
            }

            if (lot.PrixMinPourVente.HasValue && lot.PrixMinPourVente.Value < 0)
            {
                return (false, "Le prix minimum pour vente doit être supérieur ou égale à zéro.");
            }

            if (string.IsNullOrWhiteSpace(lot.Artiste))
            {
                return (false, "Le nom de l'artiste est obligatoire.");
            }

            if (lot.IdCategorie <= 0)
            {
                return (false, "L'ID de la catégorie est invalide.");
            }

            var categorieExists = await context.Categories.AnyAsync(c => c.Id == lot.IdCategorie);
            if (!categorieExists)
            {
                return (false, "La catégorie spécifiée n'existe pas.");
            }

            var vendeurExists = await context.Vendeurs.AnyAsync(v => v.Id == lot.IdVendeur);
            if (!vendeurExists)
            {
                return (false, "Le vendeur spécifié n'existe pas.");
            }

            if (lot.IdMedium <= 0)
            {
                return (false, "L'ID du medium est invalide.");
            }

            var mediumExists = await context.Mediums.AnyAsync(m => m.Id == lot.IdMedium);
            if (!mediumExists)
            {
                return (false, "Le medium spécifié n'existe pas.");
            }

            if (lot.Largeur <= 0 || lot.Hauteur <= 0)
            {
                return (false, "La largeur et la hauteur doivent être supérieures à zéro.");
            }

            // Validation spécifique pour LotModificationVM
            if (lot is LotModificationVM lotModification)
            {
                if (lotModification.IdEncanModifie.HasValue)
                {
                    var encanExists = await context.Encans.AnyAsync(e => e.Id == lotModification.IdEncanModifie.Value);
                    if (!encanExists)
                    {
                        return (false, "L'encan spécifié n'existe pas.");
                    }
                }
            }

            // Validation des photos pour la création
            if (lot.Photos != null && lot.Photos.Any(p => p.Length > 10 * 1024 * 1024)) // 10 MB max
            {
                return (false, "La taille de chaque photo ne doit pas dépasser 10 MB.");
            }

            return (true, null);
        }

        // Vous pouvez ajouter une méthode similaire pour LotModificationVM si nécessaire
    }
}
