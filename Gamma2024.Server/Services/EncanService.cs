using Gamma2024.Server.Data;
using Gamma2024.Server.Hub;
using Gamma2024.Server.Models;
using Gamma2024.Server.Validations;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Services
{
    public class EncanService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EncanService> _logger;
        private readonly IHubContext<LotMiseHub, ILotMiseHub> _hubContext;

        public EncanService(
            ApplicationDbContext context,
            ILogger<EncanService> logger,
            IHubContext<LotMiseHub, ILotMiseHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
        }

        public ICollection<EncanAffichageAdminVM> ChercherTousEncans()
        {
            var encans = _context.Encans
                .Select(e => new EncanAffichageAdminVM()
                {
                    Id = e.Id,
                    NumeroEncan = e.NumeroEncan,
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    DateDebutSoireeCloture = e.DateDebutSoireeCloture,
                    EstPublie = e.EstPublie,
                    NbLots = _context.EncanLots.Count(el => el.IdEncan == e.Id)
                }).OrderByDescending(e => e.DateDebut.Year).ThenByDescending(e => e.DateDebut.Month).ThenByDescending(e => e.DateDebut.Day).ToList();

            return encans;
        }

        public ICollection<EncanAffichageAdminVM> ChercherTousEncansVisibles()
        {
            var encans = _context.Encans
                .Where(e => e.EstPublie == true)
                .Select(e => new EncanAffichageAdminVM()
                {
                    Id = e.Id,
                    NumeroEncan = e.NumeroEncan,
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    DateDebutSoireeCloture = e.DateDebutSoireeCloture
                }).ToList();

            return encans;
        }

        public async Task<(bool Success, string Message)> CreerEncan(EncanVM vm)
        {
            var (isValid, errorMessage) = EncanValidation.ValidateEncan(vm);

            if (!isValid)
            {
                return (false, errorMessage);
            }

            var listeEncan = _context.Encans.OrderBy(e => e.NumeroEncan).ToList();

            bool estVide = false;
            if (listeEncan.Count == 0)
            {
                estVide = true;
            }

            var encan = new Encan()
            {
                Id = 0,
                NumeroEncan = estVide ? 1 : listeEncan.LastOrDefault().NumeroEncan + 1,
                DateDebut = vm.DateDebut.ToLocalTime(),
                DateFin = vm.DateFin.ToLocalTime(),
                DateDebutSoireeCloture = vm.DateFin.ToLocalTime(),
                EncanLots = [],
                EstPublie = false,
                PasLot = vm.PasLot,
                PasMise = vm.PasMise
            };
            _context.Encans.Add(encan);
            _context.SaveChanges();
            return (true, encan.Id.ToString());
        }

        public async Task<(bool Success, string Message)> MettreAJourEncanPublie(EncanPublieVM vm)
        {
            var numeroEncan = _context.Encans.FirstOrDefault(e => e.NumeroEncan.Equals(vm.NumeroEncan));
            if (numeroEncan != null)
            {
                numeroEncan.NumeroEncan = vm.NumeroEncan;
                numeroEncan.EstPublie = vm.EstPublie;
                _context.Encans.Update(numeroEncan);
                _context.SaveChanges();
                return (true, numeroEncan.Id.ToString());
            }

            return (false, "Il n'y a aucun encan à ce numéro.");
        }

        public Encan GetEncanByNumber(int numero)
        {
            return _context.Encans.FirstOrDefault(e => e.NumeroEncan == numero);
        }

        public Encan GetEncanById(int idEncan)
        {
            return _context.Encans.FirstOrDefault(e => e.Id == idEncan);
        }

        public async Task<(bool success, string message)> ModifierEncan(int id, EncanVM model)
        {
            var (isValid, message) = EncanValidation.ValidateEncan(model);
            if (!isValid)
            {
                return (false, message);
            }

            var encan = await _context.Encans.FindAsync(id);
            if (encan == null)
            {
                return (false, "Encan non trouvé");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                encan.DateDebut = model.DateDebut.ToLocalTime();
                encan.DateFin = model.DateFin.ToLocalTime();
                encan.DateDebutSoireeCloture = model.DateFin.ToLocalTime();
                encan.PasLot = model.PasLot;
                encan.PasMise = model.PasMise;


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Encan modifié avec succès");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Une erreur est survenue lors de la modification de l'encan : {ex.Message}");
            }
        }
        public EncanAffichageVM ChercherEncanParNumero(int numero)
        {
            var encan = _context.Encans
                .FirstOrDefault(e => e.NumeroEncan == numero);

            if (encan != null && encan.EstPublie)
            {
                return new EncanAffichageVM()
                {
                    Id = encan.Id,
                    NumeroEncan = encan.NumeroEncan,
                    DateDebut = encan.DateDebut,
                    DateFin = encan.DateFin,
                    DateDebutSoireeCloture = encan.DateDebutSoireeCloture
                };

            }

            return null;
        }

        public EncanAffichageVM ChercherEncanEnCours()
        {
            var encan = _context.Encans
                .FirstOrDefault(e => DateTime.Now < e.DateFin && DateTime.Now > e.DateDebut);

            if (encan != null && encan.EstPublie)
            {
                return new EncanAffichageVM()
                {
                    Id = encan.Id,
                    NumeroEncan = encan.NumeroEncan,
                    DateDebut = encan.DateDebut,
                    DateFin = encan.DateFin,
                    DateDebutSoireeCloture = encan.DateDebutSoireeCloture
                };

            }

            return null;
        }

        public int ChercherNumeroEncanEnCours()
        {
            var encan = _context.Encans
                .FirstOrDefault(e => DateTime.Now < e.DateFin && DateTime.Now > e.DateDebut);

            if (encan != null && encan.EstPublie)
            {
                return encan.NumeroEncan;

            }

            return 0;
        }

        public ICollection<EncanAffichageVM> ChercherEncansFuturs()
        {
            var encans = _context.Encans
                .Where(e => DateTime.Now < e.DateDebut)
                .Where(e => e.EstPublie == true)
                .OrderBy(e => e.DateDebut)
                .ToList()
                .Select(e => new EncanAffichageVM
                {
                    Id = e.Id,
                    NumeroEncan = e.NumeroEncan,
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    DateDebutSoireeCloture = e.DateDebutSoireeCloture
                })
                .ToList();

            return encans;
        }

        public ICollection<EncanAffichageVM> ChercherEncansPasses()
        {
            var encans = _context.Encans
                .Where(e => e.EstPublie == true && DateTime.Now > e.DateFin)
                .OrderByDescending(e => e.DateFin)
                .ToList()
                .Select(e => new EncanAffichageVM
                {
                    Id = e.Id,
                    NumeroEncan = e.NumeroEncan,
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    DateDebutSoireeCloture = e.DateDebutSoireeCloture
                })
                .ToList();

            return encans;
        }

        public async Task<(string type, Encan encan)> GetEtatCourant()
        {
            var maintenant = DateTime.Now;

            var dernierEncan = await _context.Encans
                .Include(e => e.EncanLots)
                    .ThenInclude(el => el.Lot)
                .Where(e => e.DateFin < maintenant)
                .OrderByDescending(e => e.DateFin)
                .FirstOrDefaultAsync();

            if (dernierEncan?.EstEnSoireeCloture() == true)
            {
                return ("soireeCloture", dernierEncan);
            }

            var encanCourant = await _context.Encans
                .Include(e => e.EncanLots)
                    .ThenInclude(el => el.Lot)
                        .ThenInclude(l => l.Photos)
                .Where(e => e.DateDebut <= maintenant && e.DateFin >= maintenant)
                .FirstOrDefaultAsync();

            if (encanCourant != null)
            {
                return ("courant", encanCourant);
            }

            return ("aucun", null);
        }

        public async Task<bool> EstEnSoireeCloture(int numeroEncan)
        {
            var encan = await _context.Encans
                .Include(e => e.EncanLots)
                    .ThenInclude(el => el.Lot)
                .FirstOrDefaultAsync(e => e.NumeroEncan == numeroEncan);

            return encan?.EstEnSoireeCloture() ?? false;
        }

        
    }
}
    