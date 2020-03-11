using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AddressCollector.Data.Auth;

namespace AddressCollector.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        [Required]
        public string KlantId { get; set; }
        public virtual ApplicationUser Klant { get; set; }
        [Required]
        public string OnderNemerId { get; set; }
        public virtual ApplicationUser Ondernemer { get; set; }
        [Required(ErrorMessage = "De voornaam is verplicht.")]
        public string Voornaam { get; set; }
        public string Tussenvoegsel { get; set; }
        [Required(ErrorMessage = "De achternaam is verplicht.")]
        public string Achternaan { get; set; }
        [Required(ErrorMessage = "De straat is verplicht.")]
        public string Straat { get; set; }
        [Required(ErrorMessage = "Het huisnummer is verplicht.")]
        public int Huisnummer { get; set; }
        [Display(Name = "Toevoeging")]
        public string HuisnummerToevoeging { get; set; }
        [Required(ErrorMessage = "De postcode is verplicht.")]
        public string Postcode { get; set; }
        [Required(ErrorMessage = "De plaats is verplicht.")]
        public string Plaats { get; set; }
        [Required(ErrorMessage = "Het land is verplicht.")]
        public string Land { get; set; }
        [NotMapped]
        public bool Next { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
