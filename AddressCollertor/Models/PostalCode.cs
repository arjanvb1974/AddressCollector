using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressCollector.Models
{
    public class PostalCode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string ZipCode { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Street { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string City { get; set; }
        [Required]
        [Column(TypeName = "numeric(18, 0)")]
        public decimal BeginRange { get; set; }
        [Required]
        [Column(TypeName = "numeric(18, 0)")]
        public decimal EndRange { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Even { get; set; }
    }
}
