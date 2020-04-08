using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using AddressCollector.Shared;

namespace AddressCollector.Data.Entities
{
    public class Country
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required, MaxLength(2)]
        public string Code { get; set; }

        [Required, MaxLength(Constants.FieldDefaultConstant_NameLength)]
        public string CountryName { get; set; }
    }
}
