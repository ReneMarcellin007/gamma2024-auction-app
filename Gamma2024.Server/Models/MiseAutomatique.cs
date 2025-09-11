using System.ComponentModel.DataAnnotations.Schema;

namespace Gamma2024.Server.Models
{
    public class MiseAutomatique
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public string UserId { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal Montant { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MontantMaximal { get; set; }

        public DateTime DateMise { get; set; }
        public bool EstMiseAutomatique { get; set; }

        // Navigation properties
        public virtual Lot Lot { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}