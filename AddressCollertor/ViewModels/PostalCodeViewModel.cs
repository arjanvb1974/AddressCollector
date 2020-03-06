using System.ComponentModel.DataAnnotations;

namespace AddressCollector.ViewModels
{
    public class PostalCodeViewModel
    {
        
        public int Id { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public decimal BeginRange { get; set; }
        [Required]
        public decimal EndRange { get; set; }
        [Required]
        public string Even { get; set; }
    }
}
