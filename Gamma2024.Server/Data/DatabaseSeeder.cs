using Gamma2024.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamma2024.Server.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Créer les rôles
            string[] roleNames = { "Admin", "Client", "Vendeur" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Role '{roleName}' créé.");
                }
            }

            // Créer l'utilisateur admin
            if (!context.Users.Any(u => u.Email == "admin@admin.com"))
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    Nom = "Admin",
                    Prenom = "System",
                    TelephoneNumber = "5141234567"
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine("Utilisateur admin créé.");
                }
            }

            // Créer des utilisateurs vendeurs
            var vendeurs = new List<(string email, string nom, string prenom)>
            {
                ("vendeur1@test.com", "Tremblay", "Jean"),
                ("vendeur2@test.com", "Gagnon", "Marie"),
                ("vendeur3@test.com", "Roy", "Pierre")
            };

            foreach (var (email, nom, prenom) in vendeurs)
            {
                if (!context.Users.Any(u => u.Email == email))
                {
                    var vendeur = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        Nom = nom,
                        Prenom = prenom,
                        TelephoneNumber = "5149876543"
                    };

                    var result = await userManager.CreateAsync(vendeur, "Test123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(vendeur, "Vendeur");
                        Console.WriteLine($"Vendeur {email} créé.");
                    }
                }
            }

            // Créer des utilisateurs clients
            var clients = new List<(string email, string nom, string prenom)>
            {
                ("client1@test.com", "Dubois", "Sophie"),
                ("client2@test.com", "Lavoie", "Michel"),
                ("client3@test.com", "Bergeron", "Julie")
            };

            foreach (var (email, nom, prenom) in clients)
            {
                if (!context.Users.Any(u => u.Email == email))
                {
                    var client = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        Nom = nom,
                        Prenom = prenom,
                        TelephoneNumber = "5145551234"
                    };

                    var result = await userManager.CreateAsync(client, "Test123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(client, "Client");
                        Console.WriteLine($"Client {email} créé.");
                    }
                }
            }

            // Créer des encans
            if (!context.Encans.Any())
            {
                var vendeur1 = context.Users.FirstOrDefault(u => u.Email == "vendeur1@test.com");
                var vendeur2 = context.Users.FirstOrDefault(u => u.Email == "vendeur2@test.com");
                var vendeur3 = context.Users.FirstOrDefault(u => u.Email == "vendeur3@test.com");

                var encans = new List<Encan>
                {
                    // Encan passé
                    new Encan
                    {
                        Nom = "Encan d'Art Contemporain - Été 2024",
                        Description = "Collection exceptionnelle d'œuvres d'art contemporain",
                        DateDebut = DateTime.UtcNow.AddMonths(-3),
                        DateFin = DateTime.UtcNow.AddMonths(-2),
                        CreatedOn = DateTime.UtcNow.AddMonths(-4),
                        ApplicationUserId = vendeur1?.Id
                    },
                    // Encan présent (jusqu'à décembre 2026)
                    new Encan
                    {
                        Nom = "Grande Vente d'Automne 2025",
                        Description = "Antiquités et objets de collection rares",
                        DateDebut = DateTime.UtcNow.AddDays(-10),
                        DateFin = new DateTime(2026, 12, 31, 23, 59, 59),
                        CreatedOn = DateTime.UtcNow.AddDays(-30),
                        ApplicationUserId = vendeur2?.Id
                    },
                    // Encan futur (janvier 2027 à décembre 2028)
                    new Encan
                    {
                        Nom = "Encan du Nouvel An 2027",
                        Description = "Œuvres d'art et bijoux de luxe",
                        DateDebut = new DateTime(2027, 1, 15, 0, 0, 0),
                        DateFin = new DateTime(2028, 12, 31, 23, 59, 59),
                        CreatedOn = DateTime.UtcNow,
                        ApplicationUserId = vendeur3?.Id
                    }
                };

                context.Encans.AddRange(encans);
                await context.SaveChangesAsync();
                Console.WriteLine($"{encans.Count} encans créés.");

                // Créer des lots pour chaque encan
                var categories = new[] { "Art", "Bijoux", "Antiquités", "Électronique", "Livres", "Mobilier" };
                var random = new Random();

                foreach (var encan in encans)
                {
                    var lotCount = random.Next(5, 15);
                    for (int i = 1; i <= lotCount; i++)
                    {
                        var lot = new Lot
                        {
                            Nom = $"Lot {i} - {categories[random.Next(categories.Length)]}",
                            Description = $"Description détaillée du lot {i} de l'encan {encan.Nom}",
                            PrixDepart = random.Next(50, 1000),
                            IncrementMinimum = random.Next(5, 50),
                            Categories = categories[random.Next(categories.Length)],
                            DateOuverture = encan.DateDebut.AddHours(random.Next(0, 24)),
                            DateCloture = encan.DateFin.AddDays(-random.Next(1, 10)),
                            EstVendu = encan.DateFin < DateTime.UtcNow,
                            EncanId = encan.Id,
                            CreatedOn = encan.CreatedOn
                        };

                        // Ajouter des images de test
                        lot.Image1 = "/Images/placeholder1.jpg";
                        lot.Image2 = "/Images/placeholder2.jpg";

                        context.Lots.Add(lot);
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Lots créés pour chaque encan.");

                // Créer quelques mises pour les encans actifs
                var clients1 = context.Users.Where(u => u.Email.Contains("client")).ToList();
                var lotsActifs = context.Lots.Where(l => l.DateOuverture <= DateTime.UtcNow && l.DateCloture >= DateTime.UtcNow).Take(5).ToList();

                foreach (var lot in lotsActifs)
                {
                    var miseCount = random.Next(1, 5);
                    decimal derniereMise = lot.PrixDepart;

                    for (int i = 0; i < miseCount; i++)
                    {
                        derniereMise += lot.IncrementMinimum * random.Next(1, 3);
                        var mise = new Mise
                        {
                            Montant = derniereMise,
                            DateMise = DateTime.UtcNow.AddHours(-random.Next(1, 48)),
                            LotId = lot.Id,
                            ApplicationUserId = clients1[random.Next(clients1.Count)].Id
                        };
                        context.Mises.Add(mise);
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Mises créées pour les lots actifs.");
            }

            Console.WriteLine("Seeding terminé avec succès!");
        }
    }
}