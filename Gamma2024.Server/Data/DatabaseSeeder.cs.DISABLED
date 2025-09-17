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

            // Ajouter les vendeurs SANS adresses car elles créent des conflits
            var vendeurs = new[]
            {
                new Vendeur { Id = 1, Nom = "Dupont", Prenom = "Jean", Courriel = "jean.dupont@vendeur.com", Telephone = "514-555-0001" },
                new Vendeur { Id = 2, Nom = "Martin", Prenom = "Marie", Courriel = "marie.martin@vendeur.com", Telephone = "418-555-0002" },
                new Vendeur { Id = 3, Nom = "Leblanc", Prenom = "Pierre", Courriel = "pierre.leblanc@vendeur.com", Telephone = "819-555-0003" }
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

            // Créer BEAUCOUP de lots variés
            var lots = new[]
            {
                // ENCAN EN COURS - Lots 1-8
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
                    Mise = 3200,
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
                    Mise = 7500,
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
                    Artiste = "Vincent van Gogh",
                    Description = "Champ de blé aux corbeaux - Reproduction",
                    ValeurEstimeMin = 2000,
                    ValeurEstimeMax = 3000,
                    PrixOuverture = 1500,
                    PrixMinPourVente = 1800,
                    Mise = 1600,
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
                },
                new Lot
                {
                    Id = 4,
                    Numero = "LOT-004",
                    Artiste = "Leonardo da Vinci",
                    Description = "Portrait de Mona Lisa - Copie certifiée",
                    ValeurEstimeMin = 20000,
                    ValeurEstimeMax = 30000,
                    PrixOuverture = 15000,
                    PrixMinPourVente = 18000,
                    Mise = 15500,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 1,
                    IdVendeur = 1,
                    Hauteur = 77,
                    Largeur = 53,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                },
                new Lot
                {
                    Id = 5,
                    Numero = "LOT-005",
                    Artiste = "Henri Matisse",
                    Description = "Femme au chapeau - Fauvisme",
                    ValeurEstimeMin = 8000,
                    ValeurEstimeMax = 12000,
                    PrixOuverture = 6000,
                    PrixMinPourVente = 7000,
                    Mise = 6200,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 2,
                    IdVendeur = 2,
                    Hauteur = 80,
                    Largeur = 65,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                },
                new Lot
                {
                    Id = 6,
                    Numero = "LOT-006",
                    Artiste = "Salvador Dalí",
                    Description = "La Persistance de la mémoire - Surréalisme",
                    ValeurEstimeMin = 15000,
                    ValeurEstimeMax = 25000,
                    PrixOuverture = 12000,
                    PrixMinPourVente = 14000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 1,
                    IdVendeur = 3,
                    Hauteur = 24,
                    Largeur = 33,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                },
                new Lot
                {
                    Id = 7,
                    Numero = "LOT-007",
                    Artiste = "Jackson Pollock",
                    Description = "No. 1 - Peinture gestuelle",
                    ValeurEstimeMin = 30000,
                    ValeurEstimeMax = 50000,
                    PrixOuverture = 25000,
                    PrixMinPourVente = 28000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 2,
                    IdVendeur = 1,
                    Hauteur = 200,
                    Largeur = 300,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                },
                new Lot
                {
                    Id = 8,
                    Numero = "LOT-008",
                    Artiste = "Edvard Munch",
                    Description = "Le Cri - Expressionnisme",
                    ValeurEstimeMin = 18000,
                    ValeurEstimeMax = 28000,
                    PrixOuverture = 15000,
                    PrixMinPourVente = 17000,
                    Mise = 15200,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 1,
                    IdVendeur = 2,
                    Hauteur = 91,
                    Largeur = 73,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                },

                // ENCAN PASSÉ - Lots 9-14 (vendus)
                new Lot
                {
                    Id = 9,
                    Numero = "LOT-009",
                    Artiste = "Auguste Rodin",
                    Description = "Le Penseur - Réplique en bronze",
                    ValeurEstimeMin = 3000,
                    ValeurEstimeMax = 5000,
                    PrixOuverture = 2000,
                    PrixMinPourVente = 2500,
                    Mise = 4200,
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
                    Id = 10,
                    Numero = "LOT-010",
                    Artiste = "Alberto Giacometti",
                    Description = "L'Homme qui marche - Bronze",
                    ValeurEstimeMin = 8000,
                    ValeurEstimeMax = 12000,
                    PrixOuverture = 6000,
                    PrixMinPourVente = 7000,
                    Mise = 9500,
                    EstVendu = true,
                    DateFinVente = DateTime.UtcNow.AddDays(-15),
                    EstLivrable = true,
                    IdCategorie = 2,
                    IdMedium = 3,
                    IdVendeur = 2,
                    Hauteur = 183,
                    Largeur = 95,
                    DateCreation = DateTime.UtcNow.AddDays(-40),
                    DateDepot = DateTime.UtcNow.AddDays(-35)
                },
                new Lot
                {
                    Id = 11,
                    Numero = "LOT-011",
                    Artiste = "Henry Moore",
                    Description = "Figure allongée - Marbre",
                    ValeurEstimeMin = 12000,
                    ValeurEstimeMax = 18000,
                    PrixOuverture = 10000,
                    PrixMinPourVente = 11000,
                    Mise = 15000,
                    EstVendu = true,
                    DateFinVente = DateTime.UtcNow.AddDays(-15),
                    EstLivrable = true,
                    IdCategorie = 2,
                    IdMedium = 4,
                    IdVendeur = 3,
                    Hauteur = 60,
                    Largeur = 150,
                    DateCreation = DateTime.UtcNow.AddDays(-40),
                    DateDepot = DateTime.UtcNow.AddDays(-35)
                },
                new Lot
                {
                    Id = 12,
                    Numero = "LOT-012",
                    Artiste = "Ansel Adams",
                    Description = "Moonrise over Hernandez - Photographie",
                    ValeurEstimeMin = 5000,
                    ValeurEstimeMax = 8000,
                    PrixOuverture = 4000,
                    PrixMinPourVente = 4500,
                    Mise = 6200,
                    EstVendu = true,
                    DateFinVente = DateTime.UtcNow.AddDays(-15),
                    EstLivrable = true,
                    IdCategorie = 3,
                    IdMedium = 5,
                    IdVendeur = 1,
                    Hauteur = 40,
                    Largeur = 50,
                    DateCreation = DateTime.UtcNow.AddDays(-40),
                    DateDepot = DateTime.UtcNow.AddDays(-35)
                },
                new Lot
                {
                    Id = 13,
                    Numero = "LOT-013",
                    Artiste = "Georgia O'Keeffe",
                    Description = "Red Canna - Modernisme américain",
                    ValeurEstimeMin = 7000,
                    ValeurEstimeMax = 11000,
                    PrixOuverture = 5500,
                    PrixMinPourVente = 6000,
                    Mise = 8000,
                    EstVendu = true,
                    DateFinVente = DateTime.UtcNow.AddDays(-15),
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 1,
                    IdVendeur = 2,
                    Hauteur = 91,
                    Largeur = 76,
                    DateCreation = DateTime.UtcNow.AddDays(-40),
                    DateDepot = DateTime.UtcNow.AddDays(-35)
                },
                new Lot
                {
                    Id = 14,
                    Numero = "LOT-014",
                    Artiste = "M.C. Escher",
                    Description = "Relativity - Lithographie",
                    ValeurEstimeMin = 3000,
                    ValeurEstimeMax = 5000,
                    PrixOuverture = 2500,
                    PrixMinPourVente = 2800,
                    Mise = 3800,
                    EstVendu = true,
                    DateFinVente = DateTime.UtcNow.AddDays(-15),
                    EstLivrable = true,
                    IdCategorie = 5,
                    IdMedium = 5,
                    IdVendeur = 3,
                    Hauteur = 28,
                    Largeur = 29,
                    DateCreation = DateTime.UtcNow.AddDays(-40),
                    DateDepot = DateTime.UtcNow.AddDays(-35)
                },

                // ENCAN FUTUR - Lots 15-20
                new Lot
                {
                    Id = 15,
                    Numero = "LOT-015",
                    Artiste = "Frida Kahlo",
                    Description = "Autoportrait aux épines - Surréalisme mexicain",
                    ValeurEstimeMin = 25000,
                    ValeurEstimeMax = 40000,
                    PrixOuverture = 20000,
                    PrixMinPourVente = 22000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 1,
                    IdVendeur = 1,
                    Hauteur = 61,
                    Largeur = 47,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(25),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(35)
                },
                new Lot
                {
                    Id = 16,
                    Numero = "LOT-016",
                    Artiste = "Andy Warhol",
                    Description = "Campbell's Soup Cans - Pop Art",
                    ValeurEstimeMin = 15000,
                    ValeurEstimeMax = 25000,
                    PrixOuverture = 12000,
                    PrixMinPourVente = 14000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 4,
                    IdMedium = 2,
                    IdVendeur = 2,
                    Hauteur = 51,
                    Largeur = 41,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(25),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(35)
                },
                new Lot
                {
                    Id = 17,
                    Numero = "LOT-017",
                    Artiste = "Banksy",
                    Description = "Girl with Balloon - Street Art",
                    ValeurEstimeMin = 10000,
                    ValeurEstimeMax = 18000,
                    PrixOuverture = 8000,
                    PrixMinPourVente = 9000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 4,
                    IdMedium = 2,
                    IdVendeur = 3,
                    Hauteur = 100,
                    Largeur = 70,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(25),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(35)
                },
                new Lot
                {
                    Id = 18,
                    Numero = "LOT-018",
                    Artiste = "Yves Klein",
                    Description = "IKB 191 - Bleu Klein International",
                    ValeurEstimeMin = 20000,
                    ValeurEstimeMax = 30000,
                    PrixOuverture = 18000,
                    PrixMinPourVente = 19000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 1,
                    IdMedium = 2,
                    IdVendeur = 1,
                    Hauteur = 199,
                    Largeur = 153,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(25),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(35)
                },
                new Lot
                {
                    Id = 19,
                    Numero = "LOT-019",
                    Artiste = "Kaws",
                    Description = "Companion - Sculpture contemporaine",
                    ValeurEstimeMin = 8000,
                    ValeurEstimeMax = 15000,
                    PrixOuverture = 6000,
                    PrixMinPourVente = 7000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 2,
                    IdMedium = 3,
                    IdVendeur = 2,
                    Hauteur = 130,
                    Largeur = 60,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(25),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(35)
                },
                new Lot
                {
                    Id = 20,
                    Numero = "LOT-020",
                    Artiste = "Takashi Murakami",
                    Description = "Cherry Blossom - Art contemporain japonais",
                    ValeurEstimeMin = 12000,
                    ValeurEstimeMax = 20000,
                    PrixOuverture = 10000,
                    PrixMinPourVente = 11000,
                    Mise = 0,
                    EstVendu = false,
                    EstLivrable = true,
                    IdCategorie = 4,
                    IdMedium = 2,
                    IdVendeur = 3,
                    Hauteur = 150,
                    Largeur = 150,
                    DateCreation = DateTime.UtcNow,
                    DateDepot = DateTime.UtcNow,
                    DateDebutDecompteLot = DateTime.UtcNow.AddDays(25),
                    DateFinDecompteLot = DateTime.UtcNow.AddDays(35)
                }
            };

            context.Lots.AddRange(lots);
            context.SaveChanges();

            // Associer les lots aux encans
            var encanLots = new[]
            {
                // Encan en cours (Id=1) - Lots 1-8
                new EncanLot { IdEncan = 1, IdLot = 1 },
                new EncanLot { IdEncan = 1, IdLot = 2 },
                new EncanLot { IdEncan = 1, IdLot = 3 },
                new EncanLot { IdEncan = 1, IdLot = 4 },
                new EncanLot { IdEncan = 1, IdLot = 5 },
                new EncanLot { IdEncan = 1, IdLot = 6 },
                new EncanLot { IdEncan = 1, IdLot = 7 },
                new EncanLot { IdEncan = 1, IdLot = 8 },
                
                // Encan passé (Id=2) - Lots 9-14 (vendus)
                new EncanLot { IdEncan = 2, IdLot = 9 },
                new EncanLot { IdEncan = 2, IdLot = 10 },
                new EncanLot { IdEncan = 2, IdLot = 11 },
                new EncanLot { IdEncan = 2, IdLot = 12 },
                new EncanLot { IdEncan = 2, IdLot = 13 },
                new EncanLot { IdEncan = 2, IdLot = 14 },
                
                // Encan futur (Id=3) - Lots 15-20
                new EncanLot { IdEncan = 3, IdLot = 15 },
                new EncanLot { IdEncan = 3, IdLot = 16 },
                new EncanLot { IdEncan = 3, IdLot = 17 },
                new EncanLot { IdEncan = 3, IdLot = 18 },
                new EncanLot { IdEncan = 3, IdLot = 19 },
                new EncanLot { IdEncan = 3, IdLot = 20 }
            };

            context.EncanLots.AddRange(encanLots);
            context.SaveChanges();

            // Ajouter BEAUCOUP de photos avec des URLs Placeholder d'Internet
            var photos = new[]
            {
                // Photos pour LOT-001 (Picasso)
                new Photo { Id = 1, IdLot = 1, Lien = "https://via.placeholder.com/800x600/FF6B6B/FFFFFF?text=Picasso+Nature+Morte" },
                new Photo { Id = 2, IdLot = 1, Lien = "https://via.placeholder.com/800x600/4ECDC4/FFFFFF?text=Picasso+Detail" },
                new Photo { Id = 3, IdLot = 1, Lien = "https://via.placeholder.com/800x600/45B7D1/FFFFFF?text=Picasso+Signature" },

                // Photos pour LOT-002 (Monet)
                new Photo { Id = 4, IdLot = 2, Lien = "https://via.placeholder.com/800x600/96CEB4/FFFFFF?text=Monet+Jardin" },
                new Photo { Id = 5, IdLot = 2, Lien = "https://via.placeholder.com/800x600/DDA0DD/FFFFFF?text=Monet+Giverny" },
                new Photo { Id = 6, IdLot = 2, Lien = "https://via.placeholder.com/800x600/98D8C8/FFFFFF?text=Monet+Impression" },

                // Photos pour LOT-003 (Van Gogh)
                new Photo { Id = 7, IdLot = 3, Lien = "https://via.placeholder.com/800x600/F7DC6F/333333?text=Van+Gogh+Champ" },
                new Photo { Id = 8, IdLot = 3, Lien = "https://via.placeholder.com/800x600/F8C471/333333?text=Van+Gogh+Corbeaux" },

                // Photos pour LOT-004 (Da Vinci)
                new Photo { Id = 9, IdLot = 4, Lien = "https://via.placeholder.com/800x600/D2B48C/FFFFFF?text=Mona+Lisa+Copy" },
                new Photo { Id = 10, IdLot = 4, Lien = "https://via.placeholder.com/800x600/8B7355/FFFFFF?text=Da+Vinci+Portrait" },
                new Photo { Id = 11, IdLot = 4, Lien = "https://via.placeholder.com/800x600/A0522D/FFFFFF?text=Da+Vinci+Detail" },

                // Photos pour LOT-005 (Matisse)
                new Photo { Id = 12, IdLot = 5, Lien = "https://via.placeholder.com/800x600/FF69B4/FFFFFF?text=Matisse+Femme" },
                new Photo { Id = 13, IdLot = 5, Lien = "https://via.placeholder.com/800x600/FF1493/FFFFFF?text=Matisse+Fauvisme" },

                // Photos pour LOT-006 (Dalí)
                new Photo { Id = 14, IdLot = 6, Lien = "https://via.placeholder.com/800x600/FF8C00/333333?text=Dali+Persistence" },
                new Photo { Id = 15, IdLot = 6, Lien = "https://via.placeholder.com/800x600/FF6347/FFFFFF?text=Dali+Surrealism" },
                new Photo { Id = 16, IdLot = 6, Lien = "https://via.placeholder.com/800x600/FFA500/333333?text=Dali+Melting" },

                // Photos pour LOT-007 (Pollock)
                new Photo { Id = 17, IdLot = 7, Lien = "https://via.placeholder.com/800x600/2E2E2E/FFFFFF?text=Pollock+No1" },
                new Photo { Id = 18, IdLot = 7, Lien = "https://via.placeholder.com/800x600/1C1C1C/FFFFFF?text=Pollock+Dripping" },
                new Photo { Id = 19, IdLot = 7, Lien = "https://via.placeholder.com/800x600/3C3C3C/FFFFFF?text=Pollock+Texture" },

                // Photos pour LOT-008 (Munch)
                new Photo { Id = 20, IdLot = 8, Lien = "https://via.placeholder.com/800x600/FF4500/FFFFFF?text=Munch+Le+Cri" },
                new Photo { Id = 21, IdLot = 8, Lien = "https://via.placeholder.com/800x600/DC143C/FFFFFF?text=Munch+Expression" },

                // Photos pour LOT-009 (Rodin)
                new Photo { Id = 22, IdLot = 9, Lien = "https://via.placeholder.com/800x600/8B4513/FFFFFF?text=Rodin+Penseur" },
                new Photo { Id = 23, IdLot = 9, Lien = "https://via.placeholder.com/800x600/A0522D/FFFFFF?text=Rodin+Bronze" },
                new Photo { Id = 24, IdLot = 9, Lien = "https://via.placeholder.com/800x600/6B4423/FFFFFF?text=Rodin+Profile" },

                // Photos pour LOT-010 (Giacometti)
                new Photo { Id = 25, IdLot = 10, Lien = "https://via.placeholder.com/800x600/696969/FFFFFF?text=Giacometti+Homme" },
                new Photo { Id = 26, IdLot = 10, Lien = "https://via.placeholder.com/800x600/808080/FFFFFF?text=Giacometti+Bronze" },

                // Photos pour LOT-011 (Henry Moore)
                new Photo { Id = 27, IdLot = 11, Lien = "https://via.placeholder.com/800x600/C0C0C0/333333?text=Moore+Figure" },
                new Photo { Id = 28, IdLot = 11, Lien = "https://via.placeholder.com/800x600/D3D3D3/333333?text=Moore+Marble" },
                new Photo { Id = 29, IdLot = 11, Lien = "https://via.placeholder.com/800x600/DCDCDC/333333?text=Moore+Detail" },

                // Photos pour LOT-012 (Ansel Adams)
                new Photo { Id = 30, IdLot = 12, Lien = "https://via.placeholder.com/800x600/000000/FFFFFF?text=Adams+Moonrise" },
                new Photo { Id = 31, IdLot = 12, Lien = "https://via.placeholder.com/800x600/2F4F4F/FFFFFF?text=Adams+Photography" },

                // Photos pour LOT-013 (Georgia O'Keeffe)
                new Photo { Id = 32, IdLot = 13, Lien = "https://via.placeholder.com/800x600/FF69B4/FFFFFF?text=OKeeffe+Red+Canna" },
                new Photo { Id = 33, IdLot = 13, Lien = "https://via.placeholder.com/800x600/FFB6C1/333333?text=OKeeffe+Modernism" },

                // Photos pour LOT-014 (M.C. Escher)
                new Photo { Id = 34, IdLot = 14, Lien = "https://via.placeholder.com/800x600/4169E1/FFFFFF?text=Escher+Relativity" },
                new Photo { Id = 35, IdLot = 14, Lien = "https://via.placeholder.com/800x600/0000CD/FFFFFF?text=Escher+Lithograph" },

                // Photos pour LOT-015 (Frida Kahlo)
                new Photo { Id = 36, IdLot = 15, Lien = "https://via.placeholder.com/800x600/8B008B/FFFFFF?text=Frida+Autoportrait" },
                new Photo { Id = 37, IdLot = 15, Lien = "https://via.placeholder.com/800x600/9932CC/FFFFFF?text=Frida+Epines" },
                new Photo { Id = 38, IdLot = 15, Lien = "https://via.placeholder.com/800x600/BA55D3/FFFFFF?text=Frida+Detail" },

                // Photos pour LOT-016 (Andy Warhol)
                new Photo { Id = 39, IdLot = 16, Lien = "https://via.placeholder.com/800x600/FF1493/FFFFFF?text=Warhol+Soup+Cans" },
                new Photo { Id = 40, IdLot = 16, Lien = "https://via.placeholder.com/800x600/FF69B4/FFFFFF?text=Warhol+Pop+Art" },

                // Photos pour LOT-017 (Banksy)
                new Photo { Id = 41, IdLot = 17, Lien = "https://via.placeholder.com/800x600/DC143C/FFFFFF?text=Banksy+Girl+Balloon" },
                new Photo { Id = 42, IdLot = 17, Lien = "https://via.placeholder.com/800x600/B22222/FFFFFF?text=Banksy+Street+Art" },

                // Photos pour LOT-018 (Yves Klein)
                new Photo { Id = 43, IdLot = 18, Lien = "https://via.placeholder.com/800x600/0033FF/FFFFFF?text=Klein+IKB+191" },
                new Photo { Id = 44, IdLot = 18, Lien = "https://via.placeholder.com/800x600/002FA7/FFFFFF?text=Klein+Blue" },
                new Photo { Id = 45, IdLot = 18, Lien = "https://via.placeholder.com/800x600/0047AB/FFFFFF?text=Klein+International" },

                // Photos pour LOT-019 (Kaws)
                new Photo { Id = 46, IdLot = 19, Lien = "https://via.placeholder.com/800x600/FF1493/FFFFFF?text=Kaws+Companion" },
                new Photo { Id = 47, IdLot = 19, Lien = "https://via.placeholder.com/800x600/FF69B4/333333?text=Kaws+Sculpture" },

                // Photos pour LOT-020 (Takashi Murakami)
                new Photo { Id = 48, IdLot = 20, Lien = "https://via.placeholder.com/800x600/FFD700/333333?text=Murakami+Cherry" },
                new Photo { Id = 49, IdLot = 20, Lien = "https://via.placeholder.com/800x600/FFC0CB/333333?text=Murakami+Blossom" },
                new Photo { Id = 50, IdLot = 20, Lien = "https://via.placeholder.com/800x600/FFB6C1/333333?text=Murakami+Colors" }
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