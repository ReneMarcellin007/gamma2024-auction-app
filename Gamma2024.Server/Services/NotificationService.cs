using Gamma2024.Server.Data;
using Gamma2024.Server.Hub;
using Gamma2024.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Services
{
    public class NotificationService
    {
        private readonly NotificationHub _hubContext;
        private readonly ApplicationDbContext _context;


        public NotificationService(NotificationHub hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task SendBidNotification(string userReceiptId, string message)
        {
            var notification = new Notification
            {
                ApplicationUserId = userReceiptId,
                Message = message,
                CreeA = DateTime.Now,
                EstLu = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.SendToUser(notification);
        }

        public async Task<ICollection<Notification>> GetUnreadNotification(string userId)
        {
            var notifications = _context.Notifications
                                              .Where(n => n.ApplicationUserId == userId && !n.EstLu)
                                              .OrderByDescending(n => n.CreeA)
                                              .ToList();
            if (notifications.Any())
            {
                return notifications;
            }
            return null;
        }

        public async Task<(bool Success, string Message)> MarquerNotificationLu()
        {
            var notifications = await _context.Notifications.ToListAsync();
            if (notifications == null)
            {
                return (false, "Aucune notification trouvé");
            }

            foreach (var notif in notifications)
            {
                notif.EstLu = true;
            }

            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();

            return (true, "Notification lu avec succès");
        }
    }

}
