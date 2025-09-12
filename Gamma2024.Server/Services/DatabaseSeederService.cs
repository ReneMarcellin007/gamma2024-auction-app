using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Services
{
    public class DatabaseSeederService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DatabaseSeederService> _logger;

        public DatabaseSeederService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DatabaseSeederService> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");

                // Créer les rôles
                await SeedRolesAsync();
                
                // Créer les utilisateurs
                await SeedUsersAsync();
                
                // Créer les encans
                await SeedEncansAsync();
                
                // Créer les lots
                await SeedLotsAsync();

                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database seeding");
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            string[] roleNames = { "Admin", "Client", "Vendeur" };
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                    _logger.LogInformation($"Role '{roleName}' created.");
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            // Admin
            var admin = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                Nom = "Admin",
                Prenom = "System"
            };
            
            if (await _userManager.FindByEmailAsync(admin.Email) == null)
            {
                await _userManager.CreateAsync(admin, "Admin123!");
                await _userManager.AddToRoleAsync(admin, "Admin");
                _logger.LogInformation("Admin user created.");
            }

            // Client
            var client = new ApplicationUser
            {
                UserName = "client@test.com",
                Email = "client@test.com",
                EmailConfirmed = true,
                Nom = "Client",
                Prenom = "Test"
            };
            
            if (await _userManager.FindByEmailAsync(client.Email) == null)
            {
                await _userManager.CreateAsync(client, "Client123!");
                await _userManager.AddToRoleAsync(client, "Client");
                _logger.LogInformation("Client user created.");
            }

            // Vendeur
            var vendeur = new ApplicationUser
            {
                UserName = "vendeur@test.com",
                Email = "vendeur@test.com",
                EmailConfirmed = true,
                Nom = "Vendeur",
                Prenom = "Test"
            };
            
            if (await _userManager.FindByEmailAsync(vendeur.Email) == null)
            {
                await _userManager.CreateAsync(vendeur, "Vendeur123!");
                await _userManager.AddToRoleAsync(vendeur, "Vendeur");
                _logger.LogInformation("Vendeur user created.");
            }
        }

        private async Task SeedEncansAsync()
        {
            if (!await _context.Encans.AnyAsync())
            {
                var currentDate = DateTime.Now;
                
                // Encan présent (jusqu'en décembre 2026)
                var encanPresent = new Encan
                {
                    Titre = "Grande Vente d'Antiquités 2025",
                    Description = "Collection exceptionnelle d'antiquités et d'objets d'art",
                    DateDebut = currentDate.AddDays(-1),
                    DateFin = new DateTime(2026, 12, 31, 23, 59, 59),
                    ImageUrl = "/Images/ImagesEncan234/banner.jpg",
                    EstActif = true
                };
                _context.Encans.Add(encanPresent);
                
                // Encan futur (janvier 2027 - décembre 2028)
                var encanFutur = new Encan
                {
                    Titre = "Vente Prestige 2027",
                    Description = "Vente exclusive d'objets de collection rares",
                    DateDebut = new DateTime(2027, 1, 1, 9, 0, 0),
                    DateFin = new DateTime(2028, 12, 31, 23, 59, 59),
                    ImageUrl = "/Images/ImagesEncan235/banner.jpg",
                    EstActif = true
                };
                _context.Encans.Add(encanFutur);
                
                await _context.SaveChangesAsync();
                _logger.LogInformation("Encans created successfully.");
            }
        }

        private async Task SeedLotsAsync()
        {
            if (!await _context.Lots.AnyAsync())
            {
                var vendeur = await _userManager.FindByEmailAsync("vendeur@test.com");
                var encans = await _context.Encans.ToListAsync();
                
                if (vendeur != null && encans.Any())
                {
                    var encanPresent = encans.FirstOrDefault(e => e.DateDebut <= DateTime.Now && e.DateFin >= DateTime.Now);
                    var encanFutur = encans.FirstOrDefault(e => e.DateDebut > DateTime.Now);
                    
                    // Lots pour l'encan présent
                    if (encanPresent != null)
                    {
                        var lotsPresent = new List<Lot>
                        {
                            new Lot
                            {
                                Nom = "Vase Ming Dynasty",
                                Description = "Magnifique vase de la dynastie Ming, porcelaine fine avec motifs floraux",
                                PrixDepart = 5000,
                                PrixActuel = 5000,
                                Increment = 100,
                                DatePublication = DateTime.Now.AddDays(-5),
                                EncanId = encanPresent.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            },
                            new Lot
                            {
                                Nom = "Tableau Impressionniste",
                                Description = "Huile sur toile, école française, fin XIXe siècle",
                                PrixDepart = 8000,
                                PrixActuel = 8500,
                                Increment = 250,
                                DatePublication = DateTime.Now.AddDays(-3),
                                EncanId = encanPresent.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            },
                            new Lot
                            {
                                Nom = "Montre Gousset Or",
                                Description = "Montre gousset en or 18 carats, mécanisme suisse, circa 1890",
                                PrixDepart = 3000,
                                PrixActuel = 3200,
                                Increment = 50,
                                DatePublication = DateTime.Now.AddDays(-2),
                                EncanId = encanPresent.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            },
                            new Lot
                            {
                                Nom = "Bureau Louis XV",
                                Description = "Bureau en marqueterie, époque Louis XV, excellent état",
                                PrixDepart = 12000,
                                PrixActuel = 12000,
                                Increment = 500,
                                DatePublication = DateTime.Now.AddDays(-1),
                                EncanId = encanPresent.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            },
                            new Lot
                            {
                                Nom = "Service à Thé Argent",
                                Description = "Service complet en argent massif, poinçon Minerve, 6 pièces",
                                PrixDepart = 4500,
                                PrixActuel = 4500,
                                Increment = 100,
                                DatePublication = DateTime.Now,
                                EncanId = encanPresent.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            }
                        };
                        
                        _context.Lots.AddRange(lotsPresent);
                    }
                    
                    // Lots pour l'encan futur
                    if (encanFutur != null)
                    {
                        var lotsFutur = new List<Lot>
                        {
                            new Lot
                            {
                                Nom = "Collection Timbres Rares",
                                Description = "Collection complète de timbres français 1849-1900",
                                PrixDepart = 15000,
                                PrixActuel = 15000,
                                Increment = 500,
                                DatePublication = new DateTime(2027, 1, 1),
                                EncanId = encanFutur.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            },
                            new Lot
                            {
                                Nom = "Sculpture Bronze Art Déco",
                                Description = "Sculpture en bronze signée, période Art Déco, hauteur 45cm",
                                PrixDepart = 7500,
                                PrixActuel = 7500,
                                Increment = 250,
                                DatePublication = new DateTime(2027, 1, 2),
                                EncanId = encanFutur.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            },
                            new Lot
                            {
                                Nom = "Bibliothèque Ancienne",
                                Description = "Collection de 50 livres anciens reliés cuir, XVIIIe siècle",
                                PrixDepart = 10000,
                                PrixActuel = 10000,
                                Increment = 300,
                                DatePublication = new DateTime(2027, 1, 3),
                                EncanId = encanFutur.Id,
                                VendeurId = vendeur.Id,
                                EstActif = true,
                                EstVendu = false
                            }
                        };
                        
                        _context.Lots.AddRange(lotsFutur);
                    }
                    
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Lots created successfully.");
                }
            }
        }
    }
}