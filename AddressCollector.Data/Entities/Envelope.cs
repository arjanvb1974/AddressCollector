using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AddressCollector.Data.Auth;

namespace AddressCollector.Data.Entities
{
    public class Envelope
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string OnderNemerId { get; set; }
        public virtual ApplicationUser Ondernemer { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Naam { get; set; }
        [Required]
        public int Lengte { get; set; }
        [Required]
        public int Breedte { get; set; }
        public int? OffsetLinks { get; set; }
        public int? OffsetTop { get; set; }
    }
}
