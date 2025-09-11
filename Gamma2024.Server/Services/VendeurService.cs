using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Gamma2024.Server.Validations;
using Gamma2024.Server.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Services
{
    public class VendeurService
    {
        private readonly ApplicationDbContext _context;

        public VendeurService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> CreerVendeur(VendeurVM model)
        {
            var (isValid, errorMessage) = VendeurValidation.ValidateVendeur(model);
            if (!isValid)
            {
                return (false, errorMessage);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Créer l'adresse d'abord
                var adresse = new Adresse
                {
                    Numero = int.Parse(model.Adresse.NumeroCivique),
                    Appartement = model.Adresse.Appartement,
                    Rue = model.Adresse.Rue,
                    Ville = model.Adresse.Ville,
                    Province = model.Adresse.Province,
                    Pays = model.Adresse.Pays,
                    CodePostal = model.Adresse.CodePostal
                };

                _context.Adresses.Add(adresse);
                await _context.SaveChangesAsync();

                // Ensuite, créer le vendeur avec l'ID de l'adresse
                var vendeur = new Vendeur
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Courriel = model.Courriel,
                    Telephone = model.Telephone,
                    AdresseId = adresse.Id,
                };

                _context.Vendeurs.Add(vendeur);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (true, "Vendeur créé avec succès");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log l'exception complète ici
                return (false, $"Une erreur est survenue lors de la création du vendeur : {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ModifierVendeur(int id, VendeurVM model)
        {
            var (isValid, errorMessage) = VendeurValidation.ValidateVendeur(model);
            if (!isValid)
            {
                return (false, errorMessage);
            }

            var vendeur = await _context.Vendeurs.FindAsync(id);
            if (vendeur == null)
            {
                return (false, "Vendeur non trouvé");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                vendeur.Nom = model.Nom;
                vendeur.Prenom = model.Prenom;
                vendeur.Courriel = model.Courriel;
                vendeur.Telephone = model.Telephone;

                var adresse = await _context.Adresses.FindAsync(vendeur.AdresseId);
                if (adresse != null)
                {
                    adresse.Numero = int.Parse(model.Adresse.NumeroCivique);
                    adresse.Appartement = model.Adresse.Appartement;
                    adresse.Rue = model.Adresse.Rue;
                    adresse.Ville = model.Adresse.Ville;
                    adresse.Province = model.Adresse.Province;
                    adresse.Pays = model.Adresse.Pays;
                    adresse.CodePostal = ParseCodePostal(model.Adresse.CodePostal);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Vendeur modifié avec succès");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Une erreur est survenue lors de la modification du vendeur : {ex.Message}");
            }
        }

        public async Task<VendeurVM> ObtenirVendeur(int id)
        {
            var vendeur = await _context.Vendeurs
                .Include(v => v.Adresse)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendeur == null)
            {
                return null;
            }

            return new VendeurVM
            {
                Id = vendeur.Id,
                Nom = vendeur.Nom,
                Prenom = vendeur.Prenom,
                Courriel = vendeur.Courriel,
                Telephone = vendeur.Telephone,
                Adresse = new AdresseVendeurVM
                {
                    NumeroCivique = vendeur.Adresse.Numero.ToString(),
                    Appartement = vendeur.Adresse.Appartement,
                    Rue = vendeur.Adresse.Rue,
                    Ville = vendeur.Adresse.Ville,
                    Province = vendeur.Adresse.Province,
                    Pays = vendeur.Adresse.Pays,
                    CodePostal = vendeur.Adresse.CodePostal
                }
            };
        }

        public async Task<List<VendeurVM>> ObtenirTousVendeurs()
        {
            return await _context.Vendeurs
                .Include(v => v.Adresse)
                .Select(v => new VendeurVM
                {
                    Id = v.Id,
                    Nom = v.Nom,
                    Prenom = v.Prenom,
                    Courriel = v.Courriel,
                    Telephone = v.Telephone,
                    Adresse = new AdresseVendeurVM
                    {
                        NumeroCivique = v.Adresse.Numero.ToString(),
                        Appartement = v.Adresse.Appartement,
                        Rue = v.Adresse.Rue,
                        Ville = v.Adresse.Ville,
                        Province = v.Adresse.Province,
                        Pays = v.Adresse.Pays,
                        CodePostal = v.Adresse.CodePostal
                    }
                })
                .ToListAsync();
        }

        public static string ParseCodePostal(string code)
        {
            // Nettoyer le code postal de tout espace supplémentaire
            code = code?.Trim();
            
            if (string.IsNullOrEmpty(code))
            {
                return string.Empty;
            }

            // Supprimer tous les espaces du code postal
            return code.Replace(" ", "").ToUpper();
        }
    }
}
