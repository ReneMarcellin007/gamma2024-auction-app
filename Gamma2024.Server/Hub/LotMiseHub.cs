using Gamma2024.Server.Data;
using Gamma2024.Server.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Hub
{
    public interface ILotMiseHub
    {
        Task ReceiveNewBid(object bid);
        Task UpdateBidStatus(object status);
        Task LotTempsUpdated(int lotId, DateTime nouveauTemps);
        Task LotVendu(int lotId);
    }

    public class LotMiseHub : Hub<ILotMiseHub>
    {
        private readonly ApplicationDbContext _context;
        private readonly LotService _lotService;

        public LotMiseHub(ApplicationDbContext context, LotService lotService)
        {
            _context = context;
            _lotService = lotService;
        }

        public async Task PlaceBid(int lotId, decimal amount, string userId)
        {
            // Envoyer la nouvelle mise
            await Clients.All.ReceiveNewBid(new
            {
                idLot = lotId,
                montant = amount,
                userId = userId,
                timestamp = DateTime.Now
            });

            // Récupérer l'historique des mises pour ce lot
            var previousBidders = await _context.MiseAutomatiques
                .Where(m => m.LotId == lotId)
                .OrderByDescending(m => m.DateMise)
                .Select(m => m.UserId)
                .Distinct()
                .ToListAsync();

            // Envoyer les statuts de mise à jour
            await Clients.All.UpdateBidStatus(new
            {
                lotId = lotId,
                currentBidderId = userId,
                previousBidders = previousBidders
            });
        }

        public async Task LotVendu(int lotId)
        {
            try
            {
                await _lotService.MarquerLotVendu(lotId);
            }
            catch (Exception ex)
            {
                throw new HubException($"Erreur lors du marquage du lot comme vendu: {ex.Message}");
            }
        }
    }
}
