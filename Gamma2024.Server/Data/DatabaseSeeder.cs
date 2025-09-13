using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Gamma2024.Server.Models;

namespace Gamma2024.Server.Data
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            try
            {
                using var context = new ApplicationDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

                Console.WriteLine("=== DÉBUT DU SEEDER ===");
                
                // Ne supprimer que si les tables existent déjà
                try
                {
                    if (context.EncanLots.Any()) context.EncanLots.RemoveRange(context.EncanLots);
                    if (context.Photos.Any()) context.Photos.RemoveRange(context.Photos);
                    if (context.Lots.Any()) context.Lots.RemoveRange(context.Lots);
                    if (context.Encans.Any()) context.Encans.RemoveRange(context.Encans);
                    if (context.Vendeurs.Any()) context.Vendeurs.RemoveRange(context.Vendeurs);
                    if (context.Categories.Any()) context.Categories.RemoveRange(context.Categories);
                    if (context.Mediums.Any()) context.Mediums.RemoveRange(context.Mediums);
                    context.SaveChanges();
                    Console.WriteLine("Données existantes supprimées.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Pas de données à supprimer ou erreur: {ex.Message}");
                }

            // Ajouter les catégories
            var categories = new[]
            {
                new Categorie { Id = 1, Nom = "Peinture" },
                new Categorie { Id = 2, Nom = "Sculpture" },
                new Categorie { Id = 3, Nom = "Photographie" },
                new Categorie { Id = 4, Nom = "Art numérique" },
                new Categorie { Id = 5, Nom = "Dessin" }
            };
            context.Categories.AddRange(categories);

            // Ajouter les médiums
            var mediums = new[]
            {
                new Medium { Id = 1, Type = "Huile sur toile" },
                new Medium { Id = 2, Type = "Acrylique" },
                new Medium { Id = 3, Type = "Bronze" },
                new Medium { Id = 4, Type = "Marbre" },
                new Medium { Id = 5, Type = "Photographie argentique" }
            };
            context.Mediums.AddRange(mediums);

            // Créer d'abord des adresses pour les vendeurs
            var adressesVendeurs = new[]
            {
                new Adresse { Id = 100, Numero = 100, Rue = "Rue des Artistes", Ville = "Montréal", Province = "Québec", Pays = "Canada", CodePostal = "H1H1H1", EstDomicile = false },
                new Adresse { Id = 101, Numero = 200, Rue = "Avenue des Peintres", Ville = "Québec", Province = "Québec", Pays = "Canada", CodePostal = "G1G1G1", EstDomicile = false },
                new Adresse { Id = 102, Numero = 300, Rue = "Boulevard des Arts", Ville = "Sherbrooke", Province = "Québec", Pays = "Canada", CodePostal = "J1J1J1", EstDomicile = false }
            };
            context.Adresses.AddRange(adressesVendeurs);
            context.SaveChanges();
            
            // Ajouter les vendeurs avec TOUS les champs obligatoires
            var vendeurs = new[]
            {
                new Vendeur { Id = 1, Nom = "Dupont", Prenom = "Jean", Courriel = "jean.dupont@vendeur.com", Telephone = "514-555-0001", AdresseId = 100 },
                new Vendeur { Id = 2, Nom = "Martin", Prenom = "Marie", Courriel = "marie.martin@vendeur.com", Telephone = "418-555-0002", AdresseId = 101 },
                new Vendeur { Id = 3, Nom = "Leblanc", Prenom = "Pierre", Courriel = "pierre.leblanc@vendeur.com", Telephone = "819-555-0003", AdresseId = 102 }
            };
            context.Vendeurs.AddRange(vendeurs);

            context.SaveChanges();

            // Créer un encan en cours
            var encanEnCours = new Encan
            {
                Id = 1,
                NumeroEncan = 1,
                DateDebut = DateTime.UtcNow.AddDays(-5),
                DateFin = DateTime.UtcNow.AddDays(10),
                DateDebutSoireeCloture = DateTime.UtcNow.AddDays(9),
                EstPublie = true,
                EstTermine = false,
                PasLot = 1,
                PasMise = 10
            };

            // Créer un encan passé
            var encanPasse = new Encan
            {
                Id = 2,
                NumeroEncan = 2,
                DateDebut = DateTime.UtcNow.AddDays(-30),
                DateFin = DateTime.UtcNow.AddDays(-15),
                DateDebutSoireeCloture = DateTime.UtcNow.AddDays(-16),
                EstPublie = true,
                EstTermine = true,
                PasLot = 1,
                PasMise = 10
            };

            // Créer un encan futur
            var encanFutur = new Encan
            {
                Id = 3,
                NumeroEncan = 3,
                DateDebut = DateTime.UtcNow.AddDays(20),
                DateFin = DateTime.UtcNow.AddDays(35),
                DateDebutSoireeCloture = DateTime.UtcNow.AddDays(34),
                EstPublie = true,
                EstTermine = false,
                PasLot = 1,
                PasMise = 10
            };

            context.Encans.AddRange(encanEnCours, encanPasse, encanFutur);
            context.SaveChanges();

            // Créer des lots
            var lots = new[]
            {
                new Lot
                {
                    Id = 1,
                    Numero = "LOT-001",
                    Artiste = "Pablo Picasso",
                    Description = "Nature morte aux fruits - Huile sur toile, période bleue",
                    ValeurEstimeMin = 5000,
                    ValeurEstimeMax = 8000,
                    PrixOuverture = 3000,
                    PrixMinPourVente = 4000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 1,
                    IdVendeur = 1,
                    Hauteur = 60,
                    Largeur = 80,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                },
                new Lot
                {
                    Id = 2,
                    Numero = "LOT-002",
                    Artiste = "Claude Monet",
                    Description = "Jardin à Giverny - Impression du matin",
                    ValeurEstimeMin = 10000,
                    ValeurEstimeMax = 15000,
                    PrixOuverture = 7000,
                    PrixMinPourVente = 9000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 2,
                    IdVendeur = 2,
                    Hauteur = 90,
                    Largeur = 120,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                },
                new Lot
                {
                    Id = 3,
                    Numero = "LOT-003",
                    Artiste = "Auguste Rodin",
                    Description = "Le Penseur - Réplique en bronze",
                    ValeurEstimeMin = 3000,
                    ValeurEstimeMax = 5000,
                    PrixOuverture = 2000,
                    PrixMinPourVente = 2500,
                    Mise = 1500, // Lot avec une mise (encan passé)
                    EstVendu = true,
                    DateFinVente = DateTime.UtcNow.AddDays(-15),
                    EstLivrable = true,
                    IdCategorie = 2,
                    IdMedium = 3,
                    IdVendeur = 1,
                    Hauteur = 40,
                    Largeur = 30,
                    DateCreation = DateTime.UtcNow.AddDays(-40),
                    DateDepot = DateTime.UtcNow.AddDays(-35)
                },
                new Lot
                {
                    Id = 4,
                    Numero = "LOT-004",
                    Artiste = "Vincent van Gogh",
                    Description = "Champ de blé aux corbeaux - Reproduction",
                    ValeurEstimeMin = 2000,
                    ValeurEstimeMax = 3000,
                    PrixOuverture = 1500,
                    PrixMinPourVente = 1800,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 1,
                    IdVendeur = 3,
                    Hauteur = 50,
                    Largeur = 100,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                }
            };

            context.Lots.AddRange(lots);
            context.SaveChanges();

            // Associer les lots aux encans
            var encanLots = new[]
            {
                // Lots pour l'encan en cours
                new EncanLot { IdEncan = 1, IdLot = 1 },
                new EncanLot { IdEncan = 1, IdLot = 2 },
                new EncanLot { IdEncan = 1, IdLot = 4 },
                
                // Lot pour l'encan passé
                new EncanLot { IdEncan = 2, IdLot = 3 },
                
                // Lots pour l'encan futur
                new EncanLot { IdEncan = 3, IdLot = 1 },
                new EncanLot { IdEncan = 3, IdLot = 2 }
            };

            context.EncanLots.AddRange(encanLots);
            context.SaveChanges();

            // Ajouter quelques photos d'exemple
            var photos = new[]
            {
                new Photo { Id = 1, IdLot = 1, Lien = "/images/lots/lot001_1.jpg" },
                new Photo { Id = 2, IdLot = 1, Lien = "/images/lots/lot001_2.jpg" },
                new Photo { Id = 3, IdLot = 2, Lien = "/images/lots/lot002_1.jpg" },
                new Photo { Id = 4, IdLot = 3, Lien = "/images/lots/lot003_1.jpg" },
                new Photo { Id = 5, IdLot = 4, Lien = "/images/lots/lot004_1.jpg" }
            };

            context.Photos.AddRange(photos);
            context.SaveChanges();

                Console.WriteLine("Base de données initialisée avec succès!");
                VerifierDonnees(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERREUR CRITIQUE DANS LE SEEDER: {ex.Message}");
                Console.WriteLine($"Type: {ex.GetType().Name}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public static void VerifierDonnees(ApplicationDbContext context)
        {
            Console.WriteLine("=== Vérification des données ===");
            Console.WriteLine($"Nombre d'encans: {context.Encans.Count()}");
            Console.WriteLine($"  - En cours: {context.Encans.Count(e => e.DateDebut <= DateTime.UtcNow && e.DateFin >= DateTime.UtcNow)}");
            Console.WriteLine($"  - Passés: {context.Encans.Count(e => e.DateFin < DateTime.UtcNow)}");
            Console.WriteLine($"  - Futurs: {context.Encans.Count(e => e.DateDebut > DateTime.UtcNow)}");
            Console.WriteLine($"Nombre de lots: {context.Lots.Count()}");
            Console.WriteLine($"Nombre de catégories: {context.Categories.Count()}");
            Console.WriteLine($"Nombre de médiums: {context.Mediums.Count()}");
            Console.WriteLine($"Nombre de vendeurs: {context.Vendeurs.Count()}");
        }
    }
}