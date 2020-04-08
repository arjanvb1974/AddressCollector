using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AddressCollector.Shared;

namespace AddressCollector.Models
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        
        [MaxLength(Constants.FieldDefaultConstant_NameLength)]
        public string CountryName { get; set; }

        [MaxLength(Constants.FieldDefaultConstant_Text20Length)]
        public string CountryCode { get; set; }
    }
}
