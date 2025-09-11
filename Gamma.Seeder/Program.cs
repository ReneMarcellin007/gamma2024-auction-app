using Gamma2024.Seeder;
using Gamma2024.Server.Extensions;
using Gamma2024.Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stripe;

var context = DbContextFactory.CreateDbContext();

// Mise à jour des dates d'encans pour avoir un encan présent et futur
Console.WriteLine("=== MISE À JOUR DES DATES D'ENCANS ===");
var encansList = context.Encans.OrderBy(e => e.NumeroEncan).ToList();
var now = DateTime.Now;

// Encan 234 = PRÉSENT (en cours jusqu'en décembre 2026)
var encan234 = encansList.FirstOrDefault(e => e.NumeroEncan == 234);
if (encan234 != null)
{
    encan234.DateDebut = now.AddDays(-2);
    encan234.DateFin = new DateTime(2026, 12, 31, 23, 59, 59);
    encan234.DateDebutSoireeCloture = new DateTime(2026, 12, 30, 18, 0, 0);
    encan234.EstPublie = true;
    encan234.EstTermine = false;
    Console.WriteLine("✓ Encan 234 configuré comme PRÉSENT (EN COURS jusqu'en décembre 2026)");
}

// Encan 235 = FUTUR (janvier 2027 à décembre 2028)
var encan235 = encansList.FirstOrDefault(e => e.NumeroEncan == 235);
if (encan235 != null)
{
    encan235.DateDebut = new DateTime(2027, 1, 1, 9, 0, 0);
    encan235.DateFin = new DateTime(2028, 12, 31, 23, 59, 59);
    encan235.DateDebutSoireeCloture = new DateTime(2028, 12, 30, 18, 0, 0);
    encan235.EstPublie = true;
    encan235.EstTermine = false;
    Console.WriteLine("✓ Encan 235 configuré comme FUTUR (janvier 2027 à décembre 2028)");
}

context.SaveChanges();
Console.WriteLine("✅ Dates d'encans mises à jour !");
Console.WriteLine("==========================================\n");
var builder = WebApplication.CreateBuilder(args);

// Charger les fichiers de configuration selon l'environnement
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

Console.WriteLine("Voulez vous vider la BD? Entrez 1 pour oui et autre pour non");
var choix = Console.ReadLine();

if (choix == "1")
{
    Console.WriteLine("Vider les données BD...");
    context.Encans.RemoveRange(context.Encans);
    context.Lots.RemoveRange(context.Lots);
    context.EncanLots.RemoveRange(context.EncanLots);
    context.Categories.RemoveRange(context.Categories);
    context.Mediums.RemoveRange(context.Mediums);
    context.Photos.RemoveRange(context.Photos);
    context.FactureLivraisons.RemoveRange(context.FactureLivraisons);
    context.Factures.RemoveRange(context.Factures);
    context.Charites.RemoveRange(context.Charites);
    context.Adresses.RemoveRange(context.Adresses.Where(a => a.IdApplicationUser != "8e445865-a24d-4543-a6c6-9443d048cdb9" && a.IdApplicationUser != "1d8ac862-e54d-4f10-b6f8-638808c02967"));
    context.Vendeurs.RemoveRange(context.Vendeurs);
    context.Users.RemoveRange(context.Users.Where(c => c.Id != "8e445865-a24d-4543-a6c6-9443d048cdb9" && c.Id != "1d8ac862-e54d-4f10-b6f8-638808c02967"));
    context.Notifications.RemoveRange(context.Notifications);
    context.MiseAutomatiques.RemoveRange(context.MiseAutomatiques);

    context.SaveChanges();
}
else
{
    Console.WriteLine("Garder les données du BD");
}


var usersExistants = context.Users.Include(u => u.Adresses);
var customerService = new CustomerService();
var options = new CustomerListOptions { Limit = 100 };
StripeList<Customer> customersTemp = customerService.List(options);
StripeList<Customer> customers = customersTemp;

while (customersTemp.Data.Count == 100)
{
    customers.Data.AddRange(customersTemp.Data);
    customersTemp = customerService.List(new CustomerListOptions { Limit = 100, StartingAfter = customers.Data.Last().Id });
}

Console.WriteLine("Voulez vous supprimer les clients Stripe? Entre 1 pour oui et autre pour non");
choix = Console.ReadLine();

if (choix == "1")
{
    Console.WriteLine("Suppression des clients Stripe...");
    foreach (var customer in customers.Data)
    {
        customerService.Delete(customer.Id);
    }
    customers = customerService.List(options);
    foreach (var user in usersExistants)
    {
        user.StripeCustomer = "";
        context.Users.Update(user);
        context.SaveChanges();
    }
}
else
{
    Console.WriteLine("Garder les clients Stripe");
}


foreach (var user in usersExistants)
{
    if (user.StripeCustomer.IsNullOrEmpty())
    {
        var stripeCustomer = customers.Data.Find(c => c.Email.Equals(user.Email));

        if (stripeCustomer != null)
        {
            user.StripeCustomer = stripeCustomer.Id;
        }
        else
        {
            var createOptions = new CustomerCreateOptions
            {
                Email = user.Email,
                Name = user.FirstName + " " + user.Name,
                Description = user.UserName,
                PreferredLocales = ["fr-CA"],
                PaymentMethod = "pm_card_visa",
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = "pm_card_visa",
                },
                Address = new AddressOptions
                {
                    City = user.Adresses.First().Ville,
                    Country = user.Adresses.First().Pays,
                    Line1 = $"{user.Adresses.First().Numero} {user.Adresses.First().Rue}",
                    Line2 = user.Adresses.First().Appartement,
                    PostalCode = user.Adresses.First().CodePostal,
                    State = user.Adresses.First().Province,
                },
                Phone = user.PhoneNumber,
            };
            var customer = customerService.Create(createOptions);
            user.StripeCustomer = customer.Id;
            context.Update(user);

        }
    }
}

Console.WriteLine("Début du seed...");

Console.WriteLine("Ajout des utilisateurs");


var passwordHasher = new PasswordHasher<ApplicationUser>();

var utilisateurs = System.IO.File.ReadAllLines("CSV/DonneesDec/Acheteur.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(x => x.Length > 1)
                        .ToApplicationUser()
                        .Select(u =>
                        {
                            var stripeCustomer = customers.Data.Find(c => c.Email.Equals(u.Email));

                            if (stripeCustomer != null)
                            {
                                u.StripeCustomer = stripeCustomer.Id;
                            }
                            else
                            {
                                var options = new CustomerCreateOptions()
                                {
                                    Email = u.Email,
                                    Name = u.FirstName + " " + u.Name,
                                    Description = u.UserName,
                                    PreferredLocales = ["fr-CA"],
                                    PaymentMethod = "pm_card_visa",
                                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                                    {
                                        DefaultPaymentMethod = "pm_card_visa",
                                    },
                                    Address = new AddressOptions
                                    {
                                        City = u.Adresses.First().Ville,
                                        Country = u.Adresses.First().Pays,
                                        Line1 = $"{u.Adresses.First().Numero} {u.Adresses.First().Rue}",
                                        Line2 = u.Adresses.First().Appartement,
                                        PostalCode = u.Adresses.First().CodePostal,
                                        State = u.Adresses.First().Province
                                    },
                                    Phone = u.PhoneNumber
                                };
                                var customer = customerService.Create(options);
                                u.StripeCustomer = customer.Id;
                            }

                            u.PasswordHash = passwordHasher.HashPassword(u, u.UserName + u.Adresses.First().Numero);
                            return u;
                        })
                        .ToList();

context.Users.AddRange(utilisateurs);
context.SaveChanges();

Console.WriteLine("Ajout des roles au utilisateurs");

string roleIdClient = "7da4163f-edb4-47b5-86ea-888888888888";

var utilisateursRoles = new List<IdentityUserRole<string>>();
foreach (var item in utilisateurs)
{
    utilisateursRoles.Add(new IdentityUserRole<string>
    {
        RoleId = roleIdClient,
        UserId = item.Id,
    });
}

context.UserRoles.AddRange(utilisateursRoles);
context.SaveChanges();

Console.WriteLine("Ajout des vendeurs");

var vendeurs = System.IO.File.ReadAllLines("CSV/DonneesDec/VendeursEtVentes.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                .Skip(1)
                .Where(l => l.Length > 1)
                .ToVendeur()
                .ToList();

context.Vendeurs.AddRange(vendeurs);
context.SaveChanges();

Console.WriteLine("Ajout des categories");

var categoriesLotsUniques = System.IO.File.ReadAllLines("CSV/DonneesDec/Encan232Et233.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .GetCategories()
                            .ToList();

categoriesLotsUniques.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan234.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .GetCategories()
                            .ToList());

categoriesLotsUniques.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan235.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .GetCategories()
                            .ToList());

categoriesLotsUniques = categoriesLotsUniques
                            .GroupBy(c => c.Nom)
                            .Select(c => c.First())
                            .ToList();

context.Categories.AddRange(categoriesLotsUniques);
context.SaveChanges();

Console.WriteLine("Ajout des médiums");

var mediumsLotsUniques = System.IO.File.ReadAllLines("CSV/DonneesDec/Encan232Et233.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .GetMediums()
                            .ToList();

mediumsLotsUniques.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan234.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .GetMediums()
                            .ToList());

mediumsLotsUniques.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan235.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .GetMediums()
                            .ToList());

mediumsLotsUniques = mediumsLotsUniques.GroupBy(m => m.Type).Select(m => m.First()).ToList();

context.Mediums.AddRange(mediumsLotsUniques);
context.SaveChanges();

Console.WriteLine("Ajout des lots");

var vendeursLots = System.IO.File.ReadAllLines("CSV/DonneesDec/VendeursEtVentes.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .GetVendeurLots()
                    .ToList();

var lotImages = System.IO.File.ReadAllLines("CSV/DonneesDec/Encan232Et233.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .GetLotImages()
                        .ToList();

lotImages.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan234.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .GetLotImages()
                        .ToList());

lotImages.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan235.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .GetLotImages()
                        .ToList());

var lots = System.IO.File.ReadAllLines("CSV/DonneesDec/Encan232Et233.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .ToLot()
                        .ToList();

lots.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan234.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .ToLot()
                        .ToList());

lots.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/Encan235.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .ToLot()
                        .ToList());

lots = lots.Select(l =>
                        {
                            foreach (var item in categoriesLotsUniques)
                            {
                                if (l.Lot.Categorie.Nom == item.Nom)
                                {
                                    l.Lot.IdCategorie = item.Id;
                                    l.Lot.Categorie = item;
                                    break;
                                }
                            }
                            foreach (var item in mediumsLotsUniques)
                            {
                                if (l.Lot.Medium.Type == item.Type)
                                {
                                    l.Lot.IdMedium = item.Id;
                                    l.Lot.Medium = item;
                                    break;
                                }
                            }
                            foreach (var item in vendeursLots)
                            {
                                if (item.NumeroEncan == l.NumeroEncan && item.NumerosLots.Contains(l.Lot.Numero.Trim()))
                                {
                                    l.Lot.IdVendeur = vendeurs.First(v => v.Courriel.Trim().ToLower() == item.Courriel.Trim().ToLower()).Id;
                                    l.Lot.Vendeur = vendeurs.Find(v => v.Courriel == item.Courriel);
                                    break;
                                }
                            }
                            l.Lot.DateDepot = DateTime.Now;
                            return l;
                        })
                        .ToList();

var acheteurs = System.IO.File.ReadAllLines("CSV/DonneesDec/AcheteurEncan232.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .GetAcheteurs(232)
                        .ToList();

acheteurs.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/AcheteurEncan232.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .GetAcheteurs(233)
                        .ToList());

foreach (var lot in lots)
{
    foreach (var lotImage in lotImages.ToList())
    {
        if (lotImage.NumeroEncan == lot.NumeroEncan && lotImage.NumeroLot == lot.Lot.Numero)
        {
            foreach (var image in lotImage.NomImages)
            {
                var imagePath = Path.Combine("Images", $"ImagesEncan{lot.NumeroEncan}", image);

                lot.Lot.Photos.Add(new Photo
                {
                    Lien = imagePath
                });
            }
            lotImages.Remove(lotImage);
        }
    }
}

context.Lots.AddRange(lots.Select(l => l.Lot).ToList());
context.SaveChanges();

Console.WriteLine("Ajout des encans");

var encans = new List<Encan>();

encans.Add(new Encan
{
    NumeroEncan = 232,
    DateDebut = new DateTime(2023, 3, 15, 6, 0, 0),
    DateFin = new DateTime(2023, 4, 15, 6, 0, 0),
    DateDebutSoireeCloture = new DateTime(2023, 3, 18, 6, 0, 0),
    EstPublie = true,
    PasLot = 30,
    EstTermine = true,
    PasMise = 60
});

encans.Add(new Encan
{
    NumeroEncan = 233,
    DateDebut = new DateTime(2024, 5, 18, 6, 0, 0),
    DateFin = new DateTime(2024, 6, 15, 6, 0, 0),
    DateDebutSoireeCloture = new DateTime(2024, 6, 15, 6, 0, 0),
    EstPublie = true,
    EstTermine = true,
    PasLot = 30,
    PasMise = 60
});

encans.Add(new Encan
{
    NumeroEncan = 234,
    DateDebut = new DateTime(2024, 12, 9, 8, 0, 0),
    DateFin = new DateTime(2024, 12, 16, 18, 0, 0),
    DateDebutSoireeCloture = new DateTime(2024, 12, 16, 18, 0, 0),
    EstPublie = true,
    EstTermine = false,
    PasLot = 30,
    PasMise = 60
});

encans.Add(new Encan
{
    NumeroEncan = 235,
    DateDebut = new DateTime(2025, 1, 6, 9, 0, 0),
    DateFin = new DateTime(2025, 1, 13, 18, 0, 0),
    DateDebutSoireeCloture = new DateTime(2025, 1, 13, 18, 0, 0),
    EstPublie = true,
    PasLot = 30,
    EstTermine = false,
    PasMise = 60
});


context.Encans.AddRange(encans);
context.SaveChanges();


Console.WriteLine("Association des lots au encans respectifs");
var encanLots = new List<EncanLot>();

foreach (var item in lots)
{
    encanLots.Add(new EncanLot
    {
        Lot = item.Lot,
        IdLot = item.Lot.Id,
        Encan = encans.First(e => e.NumeroEncan == item.NumeroEncan),
        IdEncan = encans.First(e => e.NumeroEncan == item.NumeroEncan).Id
    });
}

context.EncanLots.AddRange(encanLots);
context.SaveChanges();

///Ajout des decomptes 
foreach (var encan in encans)
{
    var nbLot = 0;
    foreach (var lot in encan.EncanLots.Select(el => el.Lot))
    {
        lot.DateDebutDecompteLot = encan.DateDebutSoireeCloture;
        lot.DateFinDecompteLot = encan.DateDebutSoireeCloture + TimeSpan.FromSeconds(encan.PasLot * nbLot);
        nbLot++;
    }
}

context.Encans.UpdateRange(encans);
context.Lots.UpdateRange(lots.Select(l => l.Lot));
context.SaveChanges();

Console.WriteLine("Ajout des charités");
var charites = new List<Charite>();

charites.Add(new Charite
{
    NomOrganisme = "Le phare des rives"
});
charites.Add(new Charite
{
    NomOrganisme = "Un petit pas pour l'avenir"
});
charites.Add(new Charite
{
    NomOrganisme = "Rendez-vous dans 30 ans"
});

context.Charites.AddRange(charites);
context.SaveChanges();


Console.WriteLine("Ajout des factures");
var infoFactures = System.IO.File.ReadAllLines("CSV/DonneesDec/AcheteurEncan232.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                .Skip(1)
                .Where(l => l.Length > 1)
                .GetAcheteurs(232)
                .ToList();

infoFactures.AddRange(System.IO.File.ReadAllLines("CSV/DonneesDec/AcheteurEncan233.csv", System.Text.Encoding.GetEncoding("iso-8859-1"))
                .Skip(1)
                .Where(l => l.Length > 1)
                .GetAcheteurs(233)
                .ToList());


foreach (var item in utilisateurs)
{
    var achats = infoFactures.FindAll(i => i.Pseudonyme == item.UserName).ToList();

    if (achats.Any())
    {
        var achatsParEncan = achats.GroupBy(a => a.NumeroEncan).ToList();

        foreach (var achatParEncan in achatsParEncan)
        {
            var facture = new Facture
            {
                IdClient = item.Id,
                Client = item,
                DateAchat = DateTime.Now,
                PrixLots = 0,
                NumeroEncan = 0,
                estPaye = true
            };

            foreach (var a in achatParEncan)
            {
                var lotInfo = lots.FirstOrDefault(l => l.Lot.Numero == a.Lot && l.NumeroEncan == a.NumeroEncan);
                var lot = lotInfo.Lot;
                lot.EstVendu = true;
                lot.Mise = a.PrixAchete;
                lot.IdClientMise = item.Id;
                lot.ClientMise = item;
                lot.SeraLivree = a.Livraison;
                lot.DateFinVente = DateTime.Now;
                facture.Lots.Add(lot);
                facture.NumeroEncan = achatParEncan.Key;
            }

            facture.CalculerFacture();
            context.Factures.Add(facture);
            context.SaveChanges();
        }
        infoFactures.RemoveAll(i => i.Pseudonyme == item.UserName);
    }
}

context.Lots.UpdateRange(lots.Select(l => l.Lot));
context.SaveChanges();

Console.WriteLine("Ajout des mises dans l'encan 234");

var lotsSimNumeros = new List<string> { "8", "10", "16" };
var lotsHugNumeros = new List<string> { "5", "15a", "21" };

var lotsSim = lots.Where(l => l.NumeroEncan == 234 && lotsSimNumeros.Contains(l.Lot.Numero)).Select(l => l.Lot).ToList();
var lotsHug = lots.Where(l => l.NumeroEncan == 234 && lotsHugNumeros.Contains(l.Lot.Numero)).Select(l => l.Lot).ToList();

var simon = utilisateurs.First(u => u.UserName == "SimCar");
var hugo = utilisateurs.First(u => u.UserName == "HugoLam");

foreach (var lot in lotsSim)
{
    lot.ClientMise = simon;
    lot.IdClientMise = simon.Id;
    var mise = new MiseAutomatique
    {
        LotId = lot.Id,
        UserId = simon.Id,
        Montant = (decimal)(lot.PrixOuverture),
        DateMise = DateTime.UtcNow,
        EstMiseAutomatique = false
    };
    lot.Mise = (double)mise.Montant;

    context.MiseAutomatiques.Add(mise);
    context.Lots.Update(lot);
    context.SaveChanges();
}


foreach (var lot in lotsHug)
{
    lot.ClientMise = hugo;
    lot.IdClientMise = hugo.Id;
    var mise = new MiseAutomatique
    {
        LotId = lot.Id,
        UserId = hugo.Id,
        Montant = (decimal)(lot.PrixOuverture),
        DateMise = DateTime.UtcNow,
        EstMiseAutomatique = false
    };
    lot.Mise = (double)mise.Montant;

    context.MiseAutomatiques.Add(mise);
    context.Lots.Update(lot);
    context.SaveChanges();
}

Console.WriteLine("Fin du seed");
