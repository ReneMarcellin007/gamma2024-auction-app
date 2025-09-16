using Microsoft.AspNetCore.Mvc;
using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmergencySeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmergencySeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("seed-now")]
        public async Task<IActionResult> SeedNow()
        {
            try
            {
                Console.WriteLine("🚨 EMERGENCY SEED STARTING");

                // Nettoyer d'abord
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM photos");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM encan_lots");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM lots");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM encans");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM categories");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM mediums");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM vendeurs");
                Console.WriteLine("✅ Tables nettoyées");

                // Créer les catégories nécessaires
                if (!await _context.Categories.AnyAsync())
                {
                    var categories = new[]
                    {
                        new Categorie { Nom = "Peinture" },
                        new Categorie { Nom = "Sculpture" },
                        new Categorie { Nom = "Photographie" }
                    };
                    await _context.Categories.AddRangeAsync(categories);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("✅ Catégories créées");
                }

                // Créer les médiums nécessaires
                if (!await _context.Mediums.AnyAsync())
                {
                    var mediums = new[]
                    {
                        new Medium { Type = "Huile sur toile" },
                        new Medium { Type = "Acrylique" },
                        new Medium { Type = "Bronze" }
                    };
                    await _context.Mediums.AddRangeAsync(mediums);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("✅ Médiums créés");
                }

                // Créer un vendeur par défaut
                if (!await _context.Vendeurs.AnyAsync())
                {
                    var vendeur = new Vendeur
                    {
                        Nom = "Vendeur",
                        Prenom = "Test",
                        Courriel = "test@test.com",
                        Telephone = "555-0001"
                    };
                    await _context.Vendeurs.AddRangeAsync(vendeur);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("✅ Vendeur créé");
                }

                // Créer 3 encans simples
                var encans = new List<Encan>
                {
                    new Encan
                    {
                        NumeroEncan = 1,
                        DateDebut = DateTime.UtcNow.AddDays(-5),
                        DateFin = DateTime.UtcNow.AddDays(10),
                        DateDebutSoireeCloture = DateTime.UtcNow.AddDays(9),
                        EstPublie = true,
                        EstTermine = false,
                        PasLot = 1,
                        PasMise = 10
                    },
                    new Encan
                    {
                        NumeroEncan = 2,
                        DateDebut = DateTime.UtcNow.AddDays(-30),
                        DateFin = DateTime.UtcNow.AddDays(-15),
                        DateDebutSoireeCloture = DateTime.UtcNow.AddDays(-16),
                        EstPublie = true,
                        EstTermine = true,
                        PasLot = 1,
                        PasMise = 10
                    },
                    new Encan
                    {
                        NumeroEncan = 3,
                        DateDebut = DateTime.UtcNow.AddDays(20),
                        DateFin = DateTime.UtcNow.AddDays(35),
                        DateDebutSoireeCloture = DateTime.UtcNow.AddDays(34),
                        EstPublie = true,
                        EstTermine = false,
                        PasLot = 1,
                        PasMise = 10
                    }
                };

                await _context.Encans.AddRangeAsync(encans);
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ {encans.Count} encans créés");

                // Récupérer les IDs créés
                var categorieId = (await _context.Categories.FirstAsync()).Id;
                var mediumId = (await _context.Mediums.FirstAsync()).Id;
                var vendeurId = (await _context.Vendeurs.FirstAsync()).Id;

                // Créer des lots simples
                var lots = new List<Lot>();
                var photosList = new List<Photo>();
                int photoId = 1;

                // Lots pour l'encan en cours
                for (int i = 1; i <= 8; i++)
                {
                    var lot = new Lot
                    {
                        Numero = $"LOT-{i:D3}",
                        Artiste = new[] { "Picasso", "Monet", "Van Gogh", "Da Vinci", "Matisse", "Dalí", "Pollock", "Munch" }[i - 1],
                        Description = $"Œuvre exceptionnelle #{i}",
                        ValeurEstimeMin = 1000 * i,
                        ValeurEstimeMax = 2000 * i,
                        PrixOuverture = 500 * i,
                        PrixMinPourVente = 800 * i,
                        Mise = i <= 4 ? 600 * i : 0,
                        EstVendu = false,
                        EstLivrable = true,
                        IdCategorie = categorieId,
                        IdMedium = mediumId,
                        IdVendeur = vendeurId,
                        Hauteur = 60,
                        Largeur = 80,
                        DateCreation = DateTime.UtcNow,
                        DateDepot = DateTime.UtcNow,
                        DateDebutDecompteLot = DateTime.UtcNow.AddDays(8),
                        DateFinDecompteLot = DateTime.UtcNow.AddDays(10)
                    };
                    lots.Add(lot);
                }

                // Lots pour l'encan passé
                for (int i = 9; i <= 14; i++)
                {
                    var lot = new Lot
                    {
                        Numero = $"LOT-{i:D3}",
                        Artiste = $"Artiste Passé {i}",
                        Description = $"Œuvre vendue #{i}",
                        ValeurEstimeMin = 1000 * i,
                        ValeurEstimeMax = 2000 * i,
                        PrixOuverture = 500 * i,
                        PrixMinPourVente = 800 * i,
                        Mise = 1500 * i,
                        EstVendu = true,
                        DateFinVente = DateTime.UtcNow.AddDays(-15),
                        EstLivrable = true,
                        IdCategorie = categorieId,
                        IdMedium = mediumId,
                        IdVendeur = vendeurId,
                        Hauteur = 60,
                        Largeur = 80,
                        DateCreation = DateTime.UtcNow.AddDays(-40),
                        DateDepot = DateTime.UtcNow.AddDays(-35)
                    };
                    lots.Add(lot);
                }

                // Lots pour l'encan futur
                for (int i = 15; i <= 20; i++)
                {
                    var lot = new Lot
                    {
                        Numero = $"LOT-{i:D3}",
                        Artiste = $"Artiste Futur {i}",
                        Description = $"Œuvre à venir #{i}",
                        ValeurEstimeMin = 1000 * i,
                        ValeurEstimeMax = 2000 * i,
                        PrixOuverture = 500 * i,
                        PrixMinPourVente = 800 * i,
                        Mise = 0,
                        EstVendu = false,
                        EstLivrable = true,
                        IdCategorie = categorieId,
                        IdMedium = mediumId,
                        IdVendeur = vendeurId,
                        Hauteur = 60,
                        Largeur = 80,
                        DateCreation = DateTime.UtcNow,
                        DateDepot = DateTime.UtcNow,
                        DateDebutDecompteLot = DateTime.UtcNow.AddDays(25),
                        DateFinDecompteLot = DateTime.UtcNow.AddDays(35)
                    };
                    lots.Add(lot);
                }

                await _context.Lots.AddRangeAsync(lots);
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ {lots.Count} lots créés");

                // Créer les associations EncanLot
                var encanLots = new List<EncanLot>();
                
                // Associer les lots aux encans
                for (int i = 0; i < lots.Count; i++)
                {
                    int encanId;
                    if (i < 8) encanId = encans[0].Id; // Encan en cours
                    else if (i < 14) encanId = encans[1].Id; // Encan passé
                    else encanId = encans[2].Id; // Encan futur

                    encanLots.Add(new EncanLot
                    {
                        IdEncan = encanId,
                        IdLot = lots[i].Id
                    });
                }

                await _context.EncanLots.AddRangeAsync(encanLots);
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ {encanLots.Count} associations créées");

                // Ajouter des photos pour chaque lot
                foreach (var lot in lots)
                {
                    var photo = new Photo
                    {
                        IdLot = lot.Id,
                        Lien = $"https://via.placeholder.com/800x600/{GetRandomColor()}/FFFFFF?text={Uri.EscapeDataString(lot.Artiste)}"
                    };
                    photosList.Add(photo);
                }

                await _context.Photos.AddRangeAsync(photosList);
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ {photosList.Count} photos créées");

                // Résumé final
                var totalEncans = await _context.Encans.CountAsync();
                var totalLots = await _context.Lots.CountAsync();
                var totalPhotos = await _context.Photos.CountAsync();

                return Ok(new
                {
                    success = true,
                    message = "🎉 EMERGENCY SEED RÉUSSI!",
                    data = new
                    {
                        encans = totalEncans,
                        lots = totalLots,
                        photos = totalPhotos,
                        encanEnCours = await _context.Encans.CountAsync(e => e.DateDebut <= DateTime.UtcNow && e.DateFin >= DateTime.UtcNow),
                        encansPasses = await _context.Encans.CountAsync(e => e.DateFin < DateTime.UtcNow),
                        encansFuturs = await _context.Encans.CountAsync(e => e.DateDebut > DateTime.UtcNow)
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 EMERGENCY SEED FAILED: {ex.Message}");
                return BadRequest(new
                {
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }

        private string GetRandomColor()
        {
            var colors = new[] { "FF6B6B", "4ECDC4", "45B7D1", "96CEB4", "DDA0DD", "FFD700", "FF69B4", "8A2BE2" };
            return colors[new Random().Next(colors.Length)];
        }
    }
}