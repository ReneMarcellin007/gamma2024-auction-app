using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Gamma2024.Server.Services
{
    public class AdministrateurService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrateurService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<MembreVM>> TousLesMembre()
        {
            var mesMembres = await _context.Users
                .Select(u => new MembreVM
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    Avatar = u.Avatar,
                    Telephone = u.PhoneNumber,
                    EstBloque = IsMemberLocked(u.LockoutEnd),
                    Adresses = u.Adresses.Select(a => new AdresseInfoVM
                    {
                        Id = a.Id,
                        NumeroCivique = a.Numero,
                        Rue = a.Rue,
                        Ville = a.Ville,
                        Appartement = a.Appartement,
                        CodePostal = a.CodePostal,
                        Province = a.Province,
                        Pays = a.Pays
                    }).ToList(),
                    CartesCredit = ChercherCartesStripe(u.StripeCustomer)
                }).Where(x => x.Id != "8e445865-a24d-4543-a6c6-9443d048cdb9")
                .ToListAsync();

            return mesMembres;
        }

        public async Task<MembreVM> ObtenirMembreParId(string membreId)
        {
            var membre = await _context.Users
                .Select(u => new MembreVM
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Name = u.Name,
                    FirstName = u.FirstName,
                    Avatar = u.Avatar,
                    Telephone = u.PhoneNumber,
                    Adresses = u.Adresses.Select(a => new AdresseInfoVM
                    {
                        Id = a.Id,
                        NumeroCivique = a.Numero,
                        Rue = a.Rue,
                        Ville = a.Ville,
                        Appartement = a.Appartement,
                        CodePostal = a.CodePostal,
                        Province = a.Province,
                        Pays = a.Pays
                    }).ToList(),
                    CartesCredit = ChercherCartesStripe(u.StripeCustomer)
                })
                .FirstOrDefaultAsync(x => x.Id == membreId);

            return membre;
        }

        public static bool IsMemberLocked(DateTimeOffset? date)
        {
            if (date.HasValue && date > DateTimeOffset.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static List<CarteCreditInfoVM> ChercherCartesStripe(string customerId)
        {
            var service = new PaymentMethodService();
            var options = new PaymentMethodListOptions
            {
                Type = "card",
                Limit = 100,
                Customer = customerId,
            };

            StripeList<PaymentMethod> paymentMethods = service.List(options);
            List<CarteCreditInfoVM> cartes = new();

            foreach (var paymentMethod in paymentMethods.Data)
            {
                cartes.Add(new CarteCreditInfoVM()
                {
                    Dernier4Numero = paymentMethod.Card.Last4,
                    ExpirationDate = paymentMethod.Card.ExpMonth + "/" + paymentMethod.Card.ExpYear,
                    Marque = paymentMethod.Card.Brand
                });
            }
            return cartes;
        }
    }
}
