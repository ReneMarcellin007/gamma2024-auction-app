using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gamma2024.Server.Data;
using Microsoft.Extensions.Logging;

namespace Gamma2024.Server.Hub
{
    public interface IEncanHub
    {
        Task LotTempsUpdated(int lotId, DateTime nouveauTemps);
        Task LotVendu(int lotId);
        Task SynchroniserTemps(Dictionary<int, DateTime> tempsLots);
    }

    public class EncanHub : Hub<IEncanHub>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EncanHub> _logger;

        public EncanHub(ApplicationDbContext context, ILogger<EncanHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task DemanderSynchronisation()
        {
            try
            {
                var lots = await _context.Lots
                    .Where(l => !l.EstVendu && l.DateFinDecompteLot > DateTime.Now)
                    .ToDictionaryAsync(
                        l => l.Id,
                        l => l.DateFinDecompteLot.Value
                    );

                await Clients.Caller.SynchroniserTemps(lots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la synchronisation des temps");
            }
        }

        public override async Task OnConnectedAsync()
        {
            await DemanderSynchronisation();
            await base.OnConnectedAsync();
        }

        public async Task UpdateLotTemps(int lotId, DateTime nouveauTemps)
        {
            await Clients.All.LotTempsUpdated(lotId, nouveauTemps);
        }

        public async Task LotVendu(int lotId)
        {
            await Clients.All.LotVendu(lotId);
        }
    }
}