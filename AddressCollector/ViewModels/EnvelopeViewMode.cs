using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AddressCollector.Data.Auth;
using AddressCollector.Data.Entities;
using AddressCollector.Models;

namespace AddressCollector.ViewModels
{
    public class EnvelopeViewModel
    {
        public int Id { get; set; }
        [Required]
        public string OndernemerId { get; set; }
        public virtual ApplicationUser Ondernemer { get; set; }
        [Required(ErrorMessage = "De naam is verplicht.")]
        public string Naam { get; set; }
        [Required(ErrorMessage = "De hoogte is verplicht.")]
        [Display(Name = "Hoogte (mm)")]
        public int Lengte { get; set; }
        [Required(ErrorMessage = "De breedte is verplicht.")]
        [Display(Name = "Breedte (mm)")]
        public int Breedte { get; set; }
        [Display(Name = "Offset van links (mm)")]
        public int? OffsetLinks { get; set; }
        [Display(Name = "Offset van boven (mm)")]
        public int? OffsetTop { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
