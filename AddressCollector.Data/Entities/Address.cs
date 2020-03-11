using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AddressCollector.Data.Auth;

namespace AddressCollector.Data.Entities
{
    public class Address : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string KlantId { get; set; }
        public virtual ApplicationUser Klant { get; set; }
        [Required]
        public string OnderNemerId { get; set; }
        public virtual ApplicationUser Ondernemer { get; set; }
        [Required]
        public string Voornaam { get; set; }
        public string Tussenvoegsel { get; set; }
        [Required]
        public string Achternaan { get; set; }
        [Required]
        public string Straat { get; set; }
        [Required]
        public int Huisnummer { get; set; }
        public string HuisnummerToevoeging { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Plaats { get; set; }
        [Required]
        public string Land { get; set; }
    }
}
