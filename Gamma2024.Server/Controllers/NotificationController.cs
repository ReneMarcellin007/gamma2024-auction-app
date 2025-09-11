using Gamma2024.Server.Models;
using Gamma2024.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [Authorize(Roles = ApplicationRoles.CLIENT)]
        [HttpGet("obtenirNotificationNonLu/{userId}")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId invalide");
            }

            var notifications = await _notificationService.GetUnreadNotification(userId);
            if (notifications == null)
            {
                return Ok();
            }

            return Ok(notifications);
        }


        [Authorize(Roles = ApplicationRoles.CLIENT)]
        [HttpPost("marquerLues")]
        public async Task<IActionResult> MarkAsRead()
        {

            (bool success, string message) = await _notificationService.MarquerNotificationLu();
            if (success)
            {
                return Ok(message);
            }
            return BadRequest("Erreur lors de la modification de la notification");
        }
    }
}
