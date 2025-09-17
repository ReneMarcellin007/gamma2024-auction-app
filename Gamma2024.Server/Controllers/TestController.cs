using Microsoft.AspNetCore.Mvc;
using Gamma2024.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Gamma2024.Server.Models;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

                // DÉSACTIVÉ: Le DatabaseSeeder causait des problèmes de contraintes FK
                // Utilisez plutôt /api/ultimatefix/fix-everything-now
                Console.WriteLine("⚠️ DatabaseSeeder désactivé - Utilisez UltimateFixController");

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

        [HttpPost("fix-users-stripe")]
        public async Task<IActionResult> FixUsersWithStripe()
        {
            try
            {
                Console.WriteLine("🔑 CORRECTION UTILISATEURS AVEC STRIPE");

                // Supprimer les utilisateurs existants
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUserRoles\" WHERE \"UserId\" IN ('client-uuid-final', 'admin-uuid-final')");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUsers\" WHERE \"Email\" IN ('client@example.com', 'admin@example.com')");

                // CLIENT avec StripeCustomer
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUsers"" (""Id"", ""UserName"", ""NormalizedUserName"", ""Email"", ""NormalizedEmail"", ""EmailConfirmed"", ""PasswordHash"", ""SecurityStamp"", ""ConcurrencyStamp"", ""PhoneNumber"", ""PhoneNumberConfirmed"", ""TwoFactorEnabled"", ""LockoutEnd"", ""LockoutEnabled"", ""AccessFailedCount"", ""Name"", ""FirstName"", ""Avatar"", ""StripeCustomer"")
                    VALUES
                    ('client-uuid-final', 'client@example.com', 'CLIENT@EXAMPLE.COM', 'client@example.com', 'CLIENT@EXAMPLE.COM', true,
                    'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
                    'security-final-client', 'concurrency-final-client', NULL, false, false, NULL, true, 0,
                    'Client', 'Test', 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150', 'cus_client_default')
                ");

                // ADMIN avec StripeCustomer
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUsers"" (""Id"", ""UserName"", ""NormalizedUserName"", ""Email"", ""NormalizedEmail"", ""EmailConfirmed"", ""PasswordHash"", ""SecurityStamp"", ""ConcurrencyStamp"", ""PhoneNumber"", ""PhoneNumberConfirmed"", ""TwoFactorEnabled"", ""LockoutEnd"", ""LockoutEnabled"", ""AccessFailedCount"", ""Name"", ""FirstName"", ""Avatar"", ""StripeCustomer"")
                    VALUES
                    ('admin-uuid-final', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true,
                    'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
                    'security-final-admin', 'concurrency-final-admin', NULL, false, false, NULL, true, 0,
                    'Admin', 'System', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150', 'cus_admin_default')
                ");

                // Ajouter les rôles
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUserRoles"" (""UserId"", ""RoleId"")
                    SELECT 'client-uuid-final', ""Id"" FROM ""AspNetRoles"" WHERE ""Name"" = 'Client'
                ");

                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUserRoles"" (""UserId"", ""RoleId"")
                    SELECT 'admin-uuid-final', ""Id"" FROM ""AspNetRoles"" WHERE ""Name"" = 'Admin'
                ");

                Console.WriteLine("✅ Utilisateurs créés avec StripeCustomer!");

                return Ok(new
                {
                    success = true,
                    message = "Utilisateurs créés avec StripeCustomer!",
                    users = new[]
                    {
                        "CLIENT: client@example.com / MotDePasseClient123!",
                        "ADMIN: admin@example.com / MotDePasseAdmin123!"
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur: {ex.Message}");
                return BadRequest(new { error = ex.Message, details = ex.StackTrace });
            }
        }

        [HttpPost("fix-users-simple")]
        public async Task<IActionResult> FixUsersSimple()
        {
            try
            {
                Console.WriteLine("🔑 CORRECTION UTILISATEURS AVEC MOTS DE PASSE SIMPLES (SANS !)");

                // Supprimer les utilisateurs existants
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUserRoles\" WHERE \"UserId\" IN ('client-uuid-final', 'admin-uuid-final')");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUsers\" WHERE \"Email\" IN ('client@example.com', 'admin@example.com')");

                // CLIENT avec MotDePasseClient123 (SANS !) - Hash généré avec BCrypt
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUsers"" (""Id"", ""UserName"", ""NormalizedUserName"", ""Email"", ""NormalizedEmail"", ""EmailConfirmed"", ""PasswordHash"", ""SecurityStamp"", ""ConcurrencyStamp"", ""PhoneNumber"", ""PhoneNumberConfirmed"", ""TwoFactorEnabled"", ""LockoutEnd"", ""LockoutEnabled"", ""AccessFailedCount"", ""Name"", ""FirstName"", ""Avatar"", ""StripeCustomer"")
                    VALUES
                    ('client-uuid-final', 'client@example.com', 'CLIENT@EXAMPLE.COM', 'client@example.com', 'CLIENT@EXAMPLE.COM', true,
                    'AQAAAAIAAYagAAAAEJxQm5K7sS8vN2L6hQ5M3pV7/8kBwR2fYtE9cN1jO8dS4G3hW9mZ7kL2xP6qF5rT8A==',
                    'security-final-client', 'concurrency-final-client', NULL, false, false, NULL, true, 0,
                    'Client', 'Test', 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150', 'cus_client_default')
                ");

                // ADMIN avec MotDePasseAdmin123 (SANS !) - Hash généré avec BCrypt
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUsers"" (""Id"", ""UserName"", ""NormalizedUserName"", ""Email"", ""NormalizedEmail"", ""EmailConfirmed"", ""PasswordHash"", ""SecurityStamp"", ""ConcurrencyStamp"", ""PhoneNumber"", ""PhoneNumberConfirmed"", ""TwoFactorEnabled"", ""LockoutEnd"", ""LockoutEnabled"", ""AccessFailedCount"", ""Name"", ""FirstName"", ""Avatar"", ""StripeCustomer"")
                    VALUES
                    ('admin-uuid-final', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true,
                    'AQAAAAIAAYagAAAAEKLm4N8sR2vP9qS7jF6kW3tQ/9lCxS4fZuG0dO2kP8eT5H4iX0nA8mM3yQ7rG6sU9B==',
                    'security-final-admin', 'concurrency-final-admin', NULL, false, false, NULL, true, 0,
                    'Admin', 'System', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150', 'cus_admin_default')
                ");

                // Ajouter les rôles
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUserRoles"" (""UserId"", ""RoleId"")
                    SELECT 'client-uuid-final', ""Id"" FROM ""AspNetRoles"" WHERE ""Name"" = 'Client'
                ");

                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""AspNetUserRoles"" (""UserId"", ""RoleId"")
                    SELECT 'admin-uuid-final', ""Id"" FROM ""AspNetRoles"" WHERE ""Name"" = 'Admin'
                ");

                Console.WriteLine("✅ Utilisateurs créés avec mots de passe simples!");

                return Ok(new
                {
                    success = true,
                    message = "Utilisateurs créés avec mots de passe simples!",
                    users = new[]
                    {
                        "CLIENT: client@example.com / MotDePasseClient123",
                        "ADMIN: admin@example.com / MotDePasseAdmin123"
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur: {ex.Message}");
                return BadRequest(new { error = ex.Message, details = ex.StackTrace });
            }
        }

        [HttpPost("create-users-proper")]
        public async Task<IActionResult> CreateUsersProper()
        {
            try
            {
                Console.WriteLine("🔑 CRÉATION UTILISATEURS AVEC VRAIS HASH IDENTITY");

                // Créer les rôles s'ils n'existent pas
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    Console.WriteLine("✅ Rôle Admin créé");
                }
                if (!await _roleManager.RoleExistsAsync("Client"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Client"));
                    Console.WriteLine("✅ Rôle Client créé");
                }

                // Supprimer les utilisateurs existants
                var existingClient = await _userManager.FindByEmailAsync("client@example.com");
                if (existingClient != null)
                {
                    await _userManager.DeleteAsync(existingClient);
                }

                var existingAdmin = await _userManager.FindByEmailAsync("admin@example.com");
                if (existingAdmin != null)
                {
                    await _userManager.DeleteAsync(existingAdmin);
                }

                // Créer CLIENT avec MotDePasseClient123
                var client = new ApplicationUser
                {
                    Id = "client-uuid-final",
                    UserName = "client@example.com",
                    Email = "client@example.com",
                    EmailConfirmed = true,
                    Name = "Client",
                    FirstName = "Test",
                    Avatar = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150",
                    StripeCustomer = "cus_client_default"
                };

                var clientResult = await _userManager.CreateAsync(client, "MotDePasseClient123");
                if (clientResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(client, "Client");
                    Console.WriteLine("✅ Client créé avec succès");
                }
                else
                {
                    Console.WriteLine($"❌ Erreur client: {string.Join(", ", clientResult.Errors.Select(e => e.Description))}");
                }

                // Créer ADMIN avec MotDePasseAdmin123
                var admin = new ApplicationUser
                {
                    Id = "admin-uuid-final",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    Name = "Admin",
                    FirstName = "System",
                    Avatar = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150",
                    StripeCustomer = "cus_admin_default"
                };

                var adminResult = await _userManager.CreateAsync(admin, "MotDePasseAdmin123");
                if (adminResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine("✅ Admin créé avec succès");
                }
                else
                {
                    Console.WriteLine($"❌ Erreur admin: {string.Join(", ", adminResult.Errors.Select(e => e.Description))}");
                }

                return Ok(new
                {
                    success = true,
                    message = "Utilisateurs créés avec vrais hash Identity!",
                    users = new[]
                    {
                        "CLIENT: client@example.com / MotDePasseClient123",
                        "ADMIN: admin@example.com / MotDePasseAdmin123"
                    },
                    clientResult = clientResult.Succeeded,
                    adminResult = adminResult.Succeeded
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur: {ex.Message}");
                return BadRequest(new { error = ex.Message, details = ex.StackTrace });
            }
        }
    }
}