using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gamma2024.Server.Data;
using Gamma2024.Server.Services;

namespace Gamma2024.Server.Services
{
    public class VerificationLotsBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<VerificationLotsBackgroundService> _logger;

        public VerificationLotsBackgroundService(
            IServiceProvider services,
            ILogger<VerificationLotsBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var lotService = scope.ServiceProvider.GetRequiredService<LotService>();

                        var maintenant = DateTime.Now;

                        // Chercher les encans qui devraient être terminés
                        var encansAVerifier = await context.Encans
                            .Where(e => !e.EstTermine && 
                                       e.DateFin < maintenant && 
                                       e.EncanLots.Any(el => el.Lot.Mise > 0))
                            .Include(e => e.EncanLots)
                                .ThenInclude(el => el.Lot)
                            .ToListAsync(stoppingToken);

                        foreach (var encan in encansAVerifier)
                        {
                            // Trouver le dernier lot (celui avec la plus grande DateFinDecompteLot)
                            var dernierLot = encan.EncanLots
                                .Select(el => el.Lot)
                                .Where(l => l.DateFinDecompteLot.HasValue)
                                .OrderByDescending(l => l.DateFinDecompteLot)
                                .FirstOrDefault();

                            if (dernierLot == null || dernierLot.DateFinDecompteLot > maintenant)
                            {
                                // Si le dernier lot n'est pas encore terminé, on passe à l'encan suivant
                                continue;
                            }

                            _logger.LogInformation($"Traitement de l'encan #{encan.NumeroEncan} - Dernier lot {dernierLot.Numero} se termine à {dernierLot.DateFinDecompteLot}");

                            // Traiter tous les lots expirés dans l'ordre chronologique
                            var lotsExpires = encan.EncanLots
                                .Select(el => el.Lot)
                                .Where(l => !l.EstVendu && 
                                          l.DateFinDecompteLot <= maintenant &&
                                          l.Mise > 0 &&
                                          l.IdClientMise != null)
                                .OrderBy(l => l.DateFinDecompteLot)
                                .ToList();

                            foreach (var lot in lotsExpires)
                            {
                                await lotService.MarquerLotVenduInterne(lot.Id);
                                _logger.LogInformation($"Lot {lot.Numero} marqué comme vendu");
                            }

                            // Si on vient de traiter le dernier lot, finaliser l'encan
                            if (lotsExpires.Any(l => l.Id == dernierLot.Id))
                            {
                                _logger.LogInformation($"Dernier lot traité, finalisation de l'encan #{encan.NumeroEncan}");
                                await lotService.FinaliserEncan(encan.NumeroEncan);
                            }
                        }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la vérification des lots");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}
