namespace Gamma2024.Server.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = default!;
        public bool EstLu { get; set; }
        public DateTime CreeA { get; set; }

        public string ApplicationUserId { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
