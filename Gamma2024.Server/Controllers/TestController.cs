using Microsoft.AspNetCore.Mvc;
using Gamma2024.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("force-seed")]
        public async Task<IActionResult> ForceSeed()
        {
            try
            {
                Console.WriteLine("🔥 FORCE SEEDING DÉMARRÉ");
                
                // Tester la connexion
                var canConnect = await _context.Database.CanConnectAsync();
                Console.WriteLine($"Connexion DB: {canConnect}");
                
                if (!canConnect)
                {
                    Console.WriteLine("❌ Pas de connexion à la base de données");
                    return BadRequest("Pas de connexion DB");
                }

                // Forcer la création des tables
                await _context.Database.EnsureCreatedAsync();
                Console.WriteLine("✅ Tables créées/vérifiées");

                // Exécuter le seeder
                DatabaseSeeder.SeedDatabase(HttpContext.RequestServices);
                Console.WriteLine("✅ Seeder exécuté");

                // Vérifier les résultats
                var encanCount = await _context.Encans.CountAsync();
                var lotCount = await _context.Lots.CountAsync();
                var photoCount = await _context.Photos.CountAsync();

                var result = new
                {
                    success = true,
                    message = "Seeding forcé réussi!",
                    data = new
                    {
                        encans = encanCount,
                        lots = lotCount,
                        photos = photoCount
                    }
                };

                Console.WriteLine($"🎉 RÉSULTAT: {encanCount} encans, {lotCount} lots, {photoCount} photos");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 ERREUR FORCE SEED: {ex.Message}");
                Console.WriteLine($"💥 Type: {ex.GetType().Name}");
                Console.WriteLine($"💥 Stack: {ex.StackTrace}");
                
                return BadRequest(new
                {
                    error = ex.Message,
                    type = ex.GetType().Name,
                    stack = ex.StackTrace
                });
            }
        }

        [HttpGet("check-db")]
        public async Task<IActionResult> CheckDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var encanCount = await _context.Encans.CountAsync();
                var lotCount = await _context.Lots.CountAsync();
                var photoCount = await _context.Photos.CountAsync();

                return Ok(new
                {
                    connected = canConnect,
                    encans = encanCount,
                    lots = lotCount,
                    photos = photoCount
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}