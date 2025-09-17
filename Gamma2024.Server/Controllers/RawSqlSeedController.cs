using Microsoft.AspNetCore.Mvc;
using Gamma2024.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RawSqlSeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RawSqlSeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("populate-now")]
        public async Task<IActionResult> PopulateNow()
        {
            try
            {
                Console.WriteLine("🚨 RAW SQL SEED STARTING");

                // ÉTAPE 1: Nettoyer complètement
                var cleanupSql = @"
                    DELETE FROM photos;
                    DELETE FROM encan_lots;
                    DELETE FROM lots;
                    DELETE FROM encans;
                    DELETE FROM vendeurs;
                    DELETE FROM adresses;
                    DELETE FROM categories WHERE ""Id"" >= 100;
                    DELETE FROM mediums WHERE ""Id"" >= 100;
                ";
                await _context.Database.ExecuteSqlRawAsync(cleanupSql);
                Console.WriteLine("✅ Cleanup completed");

                // ÉTAPE 2: Créer l'adresse
                var adresseSql = @"
                    INSERT INTO adresses (""Id"", ""Numero"", ""Rue"", ""Ville"", ""Province"", ""Pays"", ""CodePostal"", ""EstDomicile"")
                    VALUES (100, 123, 'Rue Test', 'Montreal', 'Quebec', 'Canada', 'H1H1H1', false);
                ";
                await _context.Database.ExecuteSqlRawAsync(adresseSql);
                Console.WriteLine("✅ Address created");

                // ÉTAPE 3: Créer le vendeur
                var vendeurSql = @"
                    INSERT INTO vendeurs (""Id"", ""Nom"", ""Prenom"", ""Courriel"", ""Telephone"", ""AdresseId"")
                    VALUES (100, 'Vendeur', 'Test', 'test@test.com', '555-0001', 100);
                ";
                await _context.Database.ExecuteSqlRawAsync(vendeurSql);
                Console.WriteLine("✅ Vendor created");

                // ÉTAPE 4: Créer catégories et médiums
                var categoriesSql = @"
                    INSERT INTO categories (""Id"", ""Nom"") VALUES
                    (100, 'Peinture'),
                    (101, 'Sculpture'),
                    (102, 'Photographie');
                ";
                await _context.Database.ExecuteSqlRawAsync(categoriesSql);

                var mediumsSql = @"
                    INSERT INTO mediums (""Id"", ""Type"") VALUES
                    (100, 'Huile sur toile'),
                    (101, 'Acrylique'),
                    (102, 'Bronze');
                ";
                await _context.Database.ExecuteSqlRawAsync(mediumsSql);
                Console.WriteLine("✅ Categories and mediums created");

                // ÉTAPE 5: Créer les encans
                var encansSql = @"
                    INSERT INTO encans (""Id"", ""NumeroEncan"", ""DateDebut"", ""DateFin"", ""DateDebutSoireeCloture"", ""EstPublie"", ""EstTermine"", ""PasLot"", ""PasMise"")
                    VALUES
                    (100, 1, NOW() - INTERVAL '5 days', NOW() + INTERVAL '10 days', NOW() + INTERVAL '9 days', true, false, 1, 10),
                    (101, 2, NOW() - INTERVAL '30 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '16 days', true, true, 1, 10),
                    (102, 3, NOW() + INTERVAL '20 days', NOW() + INTERVAL '35 days', NOW() + INTERVAL '34 days', true, false, 1, 10);
                ";
                await _context.Database.ExecuteSqlRawAsync(encansSql);
                Console.WriteLine("✅ Auctions created");

                // ÉTAPE 6: Créer les lots
                var lotsSql = @"
                    INSERT INTO lots (""Id"", ""Numero"", ""Artiste"", ""Description"", ""ValeurEstimeMin"", ""ValeurEstimeMax"", ""PrixOuverture"", ""PrixMinPourVente"", ""Mise"", ""EstVendu"", ""EstLivrable"", ""IdCategorie"", ""IdMedium"", ""IdVendeur"", ""Hauteur"", ""Largeur"", ""DateCreation"", ""DateDepot"", ""DateDebutDecompteLot"", ""DateFinDecompteLot"")
                    VALUES
                    (100, 'LOT-001', 'Pablo Picasso', 'Nature morte aux fruits - Période bleue', 5000, 8000, 3000, 4000, 3200, false, true, 100, 100, 100, 60, 80, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
                    (101, 'LOT-002', 'Claude Monet', 'Jardin à Giverny - Impression du matin', 10000, 15000, 7000, 9000, 7500, false, true, 100, 100, 100, 90, 120, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
                    (102, 'LOT-003', 'Vincent van Gogh', 'Champ de blé aux corbeaux', 2000, 3000, 1500, 1800, 1600, false, true, 100, 100, 100, 50, 100, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
                    (103, 'LOT-004', 'Leonardo da Vinci', 'Portrait de Mona Lisa - Copie certifiée', 20000, 30000, 15000, 18000, 15500, false, true, 100, 100, 100, 77, 53, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
                    (104, 'LOT-005', 'Henri Matisse', 'Femme au chapeau - Fauvisme', 8000, 12000, 6000, 7000, 6200, false, true, 100, 101, 100, 80, 65, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
                    (105, 'LOT-006', 'Salvador Dalí', 'La Persistance de la mémoire', 15000, 25000, 12000, 14000, 0, false, true, 100, 100, 100, 24, 33, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
                    (106, 'LOT-007', 'Jackson Pollock', 'No. 1 - Peinture gestuelle', 30000, 50000, 25000, 28000, 0, false, true, 100, 101, 100, 200, 300, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
                    (107, 'LOT-008', 'Edvard Munch', 'Le Cri - Expressionnisme', 18000, 28000, 15000, 17000, 15200, false, true, 100, 100, 100, 91, 73, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days');
                ";
                await _context.Database.ExecuteSqlRawAsync(lotsSql);
                Console.WriteLine("✅ Lots created");

                // ÉTAPE 7: Associer lots aux encans
                var associationsSql = @"
                    INSERT INTO encan_lots (""IdEncan"", ""IdLot"")
                    VALUES
                    (100, 100), (100, 101), (100, 102), (100, 103),
                    (100, 104), (100, 105), (100, 106), (100, 107);
                ";
                await _context.Database.ExecuteSqlRawAsync(associationsSql);
                Console.WriteLine("✅ Associations created");

                // ÉTAPE 8: Créer les photos
                var photosSql = @"
                    INSERT INTO photos (""Id"", ""IdLot"", ""Lien"")
                    VALUES
                    (100, 100, 'https://via.placeholder.com/800x600/FF6B6B/FFFFFF?text=Picasso+Nature+Morte'),
                    (101, 101, 'https://via.placeholder.com/800x600/4ECDC4/FFFFFF?text=Monet+Giverny'),
                    (102, 102, 'https://via.placeholder.com/800x600/45B7D1/FFFFFF?text=Van+Gogh+Corbeaux'),
                    (103, 103, 'https://via.placeholder.com/800x600/96CEB4/FFFFFF?text=Da+Vinci+Mona+Lisa'),
                    (104, 104, 'https://via.placeholder.com/800x600/DDA0DD/FFFFFF?text=Matisse+Fauvisme'),
                    (105, 105, 'https://via.placeholder.com/800x600/FF69B4/FFFFFF?text=Dali+Persistance'),
                    (106, 106, 'https://via.placeholder.com/800x600/8A2BE2/FFFFFF?text=Pollock+No1'),
                    (107, 107, 'https://via.placeholder.com/800x600/FFD700/333333?text=Munch+Le+Cri');
                ";
                await _context.Database.ExecuteSqlRawAsync(photosSql);
                Console.WriteLine("✅ Photos created");

                // VÉRIFICATION FINALE
                var verificationSql = @"
                    SELECT
                        (SELECT COUNT(*) FROM encans) as encans,
                        (SELECT COUNT(*) FROM lots) as lots,
                        (SELECT COUNT(*) FROM photos) as photos,
                        (SELECT COUNT(*) FROM encan_lots) as associations,
                        (SELECT COUNT(*) FROM encans WHERE ""DateDebut"" <= NOW() AND ""DateFin"" >= NOW()) as encans_en_cours;
                ";

                var counts = await _context.Database.SqlQueryRaw<dynamic>(verificationSql).ToListAsync();

                return Ok(new
                {
                    success = true,
                    message = "🎉 RAW SQL SEED RÉUSSI!",
                    timestamp = DateTime.UtcNow,
                    summary = "Base de données peuplée avec Picasso, Monet, Van Gogh, etc."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 RAW SQL SEED FAILED: {ex.Message}");
                Console.WriteLine($"💥 Stack: {ex.StackTrace}");

                return BadRequest(new
                {
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }
    }
}